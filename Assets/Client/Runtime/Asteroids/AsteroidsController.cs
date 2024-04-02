using System;

using Client.ControllerContainer;
using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.Services;
using Client.Runtime.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Client.Runtime.Asteroids
{
    public class AsteroidsController : IInitController, IRunController, IDisposable
    {
        private readonly AsteroidsRepository _asteroidsRepository = ServiceLocator<AsteroidsRepository>.Get();
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();
        private readonly ScreenService _screenService = ServiceLocator<ScreenService>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();

        private float _spawnTimer;
        private PlayerModel _player;

        void IInitController.Init()
        {
            _player = _playerRepository.Player;
            _eventBus.Subscribe<BulletsCollisionEvt>(HandleBulletCollision);
        }

        void IRunController.Run()
        {
            Spawn();
            Update();
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<BulletsCollisionEvt>(HandleBulletCollision);
        }

        private void Spawn()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                var spawnRate = _gameplayConfig.AsteroidsSpawnRate;
                var angleOffset = _gameplayConfig.AsteroidsSpawnAngleOffset;
                _spawnTimer = Random.Range(spawnRate.x, spawnRate.y);
                var minBounds = _screenService.MinBounds;
                var maxBounds = _screenService.MaxBounds;
                var spawnPosition = _screenService.GetRandomPositionOnScreenSide();
                var asteroid = _asteroidsRepository.Get();
                asteroid.Position = spawnPosition;

                var directionToPlayer = (_player.Position - asteroid.Position).normalized;
                var randomAngle = Random.Range(-angleOffset, angleOffset);
                var offsetRotation = Quaternion.Euler(0, 0, randomAngle);
                var desiredDirection = offsetRotation * directionToPlayer;
                var velocityRange = _gameplayConfig.AsteroidsVelocityRange;
                asteroid.Velocity = desiredDirection * Random.Range(velocityRange.x, velocityRange.y);

                var size = Random.Range(0f, 1f) > 0.5f ? AsteroidSize.Big : AsteroidSize.Small;
                asteroid.Size = size;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _asteroidsRepository.ActiveElements.Count; i++)
            {
                var asteroid = _asteroidsRepository.ActiveElements[i];
                asteroid.Position += asteroid.Velocity * Time.deltaTime;
                if (_screenService.IsOutOfScreen(asteroid.Position))
                {
                    _asteroidsRepository.Release(asteroid);
                    continue;
                }

                Rotate(asteroid);
            }
        }

        private void HandleBulletCollision(BulletsCollisionEvt collisionEvt)
        {
            if (collisionEvt.Object is not AsteroidModel asteroid)
                return;

            TrySpawnShards(asteroid);
            _asteroidsRepository.Release(asteroid);
        }

        private void TrySpawnShards(AsteroidModel asteroid)
        {
            if (asteroid.Size != AsteroidSize.Big) return;
            if (Random.Range(0f, 1f) > _gameplayConfig.ShardsSpawnChance) return;

            var spawnRange = _gameplayConfig.ShardsSpawnRange;
            var velocityRange = _gameplayConfig.AsteroidsVelocityRange;
            var shardsCount = Random.Range(spawnRange.x, spawnRange.y);
            for (int i = 0; i < shardsCount; i++)
            {
                var shard = _asteroidsRepository.Get();
                var randomAngle = Random.Range(-90f, 90f);
                var direction = Quaternion.Euler(0, 0, randomAngle) * asteroid.Velocity.normalized;
                shard.Size = AsteroidSize.Small;
                shard.Velocity = direction * Random.Range(velocityRange.x, velocityRange.y);
                shard.Position = asteroid.Position;
                shard.Position += shard.Velocity * (Time.deltaTime * 30);
            }
        }

        private void Rotate(AsteroidModel asteroid)
        {
            var angle = asteroid.Size == AsteroidSize.Big
                ? _gameplayConfig.BigAsteroidRotationSpeed * Time.deltaTime
                : _gameplayConfig.SmallAsteroidRotationSpeed * Time.deltaTime;

            asteroid.Rotate(angle);
        }
    }
}