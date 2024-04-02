using System.Collections.Generic;

using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.UFO
{
    public class UfoAIBehaviour
    {
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly BulletsRepository _bulletsRepository = ServiceLocator<BulletsRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();

        private Vector2 _desiredPosition;
        private float _shootingTimer;

        public Queue<Vector2> MovePath { get; } = new();
        public float MoveSpeed { get; set; }

        public void Update(UfoModel ufo)
        {
            MoveByPath(ufo);
            ShootToPlayer(ufo);
        }

        public void Reset()
        {
            _desiredPosition = Vector2.negativeInfinity;
            MovePath.Clear();
            MoveSpeed = 0;
        }

        private void MoveByPath(UfoModel ufo)
        {
            if (MovePath.Count == 0)
                return;

            if (_desiredPosition.Equals(Vector2.negativeInfinity))
                _desiredPosition = MovePath.Dequeue();

            if ((_desiredPosition - ufo.Position).sqrMagnitude < 0.1f)
                _desiredPosition = MovePath.Dequeue();

            ufo.Velocity = (_desiredPosition - ufo.Position).normalized * MoveSpeed;
        }

        private void ShootToPlayer(UfoModel ufo)
        {
            _shootingTimer += Time.deltaTime;

            if (_shootingTimer < _gameplayConfig.UfoShootingInterval)
                return;

            _shootingTimer = 0;

            var playerPosition = _playerRepository.Player.Position;
            var playerVelocity = _playerRepository.Player.Velocity;
            var bulletSpeed = _gameplayConfig.BulletSpeed;
            var directionToPlayer = playerPosition - ufo.Position;
            var timeToHit = directionToPlayer.magnitude / bulletSpeed;
            var predictedPlayerPosition = playerPosition + playerVelocity * timeToHit;
            var bulletVelocity = (predictedPlayerPosition - ufo.Position).normalized * bulletSpeed;
            var bullet = _bulletsRepository.Get();
            bullet.Velocity = bulletVelocity;
            bullet.Position = ufo.Position + bulletVelocity * Time.deltaTime * 20;
            bullet.Rotation = Quaternion.FromToRotation(Vector2.up, bulletVelocity.normalized);
            bullet.Shooter = ufo;
        }
    }
}