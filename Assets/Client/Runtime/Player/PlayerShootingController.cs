using Client.ControllerContainer;
using Client.Runtime.Config;
using Client.Runtime.Services;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Bullets
{
    public class PlayerShootingController : IInitController, IRunController
    {
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly BulletsRepository _bulletsRepository = ServiceLocator<BulletsRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();
        private readonly Input _input = ServiceLocator<Input>.Get();
        private readonly ScreenService _screenService = ServiceLocator<ScreenService>.Get();

        private PlayerModel _player;

        void IInitController.Init()
        {
            _input.Enable();
            _player = _playerRepository.Player;
        }

        void IRunController.Run()
        {
            HandleFireInput();
            UpdateBullets();
        }

        private void HandleFireInput()
        {
            if (!_input.Player.Fire.WasPerformedThisFrame())
                return;

            var bullet = _bulletsRepository.Get();
            bullet.Position = _player.BulletSpawnPosition;
            bullet.Rotation = _player.Rotation;
            bullet.Velocity = _player.ForwardDirection * _gameplayConfig.BulletSpeed;
            bullet.Shooter = _player;
        }

        private void UpdateBullets()
        {
            for (int i = 0; i < _bulletsRepository.ActiveElements.Count; i++)
            {
                var bullet = _bulletsRepository.ActiveElements[i];
                bullet.Position += bullet.Velocity * Time.deltaTime;
                if (_screenService.IsOutOfScreen(bullet.Position))
                {
                    _bulletsRepository.Release(bullet);
                }
            }
        }
    }
}