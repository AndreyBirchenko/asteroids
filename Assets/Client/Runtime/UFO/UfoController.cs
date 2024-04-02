using System;

using Client.ControllerContainer;
using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.Services;
using Client.Runtime.Utils;

using UnityEngine;
using UnityEngine.Pool;

using Random = UnityEngine.Random;

namespace Client.Runtime.UFO
{
    public class UfoController : IInitController, IRunController, IDisposable
    {
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly UfoRepository _ufoRepository = ServiceLocator<UfoRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();
        private readonly ScreenService _screenService = ServiceLocator<ScreenService>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();

        private PlayerModel _player;
        private float _spawnTimer;
        private ObjectPool<UfoAIBehaviour> _aiBehaviourPool;

        void IInitController.Init()
        {
            _player = _playerRepository.Player;
            _spawnTimer = _gameplayConfig.UfoSpawnRate.x;
            _aiBehaviourPool = new ObjectPool<UfoAIBehaviour>(() => new UfoAIBehaviour(), x => x.Reset());
            _eventBus.Subscribe<BulletsCollisionEvt>(HandleBulletCollision);
        }

        void IRunController.Run()
        {
            Spawn();
            Update();
        }

        void IDisposable.Dispose()
        {
            _aiBehaviourPool?.Dispose();
        }

        private void Spawn()
        {
            if (_ufoRepository.ActiveElements.Count > 0)
                return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                var spawnRate = _gameplayConfig.UfoSpawnRate;
                _spawnTimer = Random.Range(spawnRate.x, spawnRate.y);
                var spawnPosition = _screenService.GetRandomPositionOnScreenSide();
                var ufo = _ufoRepository.Get();
                ufo.Position = spawnPosition;

                var aiBehaviour = _aiBehaviourPool.Get();
                ufo.AIBehaviour = aiBehaviour;
                aiBehaviour.MovePath.Enqueue(_player.Position);
                aiBehaviour.MovePath.Enqueue(_screenService.GetRandomPositionOnScreenSide());
                aiBehaviour.MoveSpeed = _gameplayConfig.UfoMoveSpeed;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _ufoRepository.ActiveElements.Count; i++)
            {
                var ufo = _ufoRepository.ActiveElements[i];
                ufo.AIBehaviour.Update(ufo);
                ufo.Position += ufo.Velocity * Time.deltaTime;
                if (_screenService.IsOutOfScreen(ufo.Position))
                    _ufoRepository.Release(ufo);
            }
        }

        private void HandleBulletCollision(BulletsCollisionEvt collisionEvt)
        {
            if (collisionEvt.Object is not UfoModel ufo)
                return;

            _ufoRepository.Release(ufo);
        }
    }
}