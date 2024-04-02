using Client.ControllerContainer;
using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.Laser;
using Client.Runtime.Services;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Player
{
    public class PlayerLaserController : IInitController, IRunController
    {
        private readonly LaserRepository _laserRepository = ServiceLocator<LaserRepository>.Get();
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();
        private readonly Input _input = ServiceLocator<Input>.Get();
        private readonly ScreenService _screenService = ServiceLocator<ScreenService>.Get();

        private PlayerModel _player;
        private float _currentShootDuration;

        void IInitController.Init()
        {
            _player = _playerRepository.Player;
            _player.LaserCooldown = _gameplayConfig.LaserCooldown;
        }

        void IRunController.Run()
        {
            Recharge();
            HandleInput();
            Update();
        }

        private void HandleInput()
        {
            if (!_input.Player.Laser.WasPerformedThisFrame()) return;
            if (_player.LaserCharges == 0) return;
            if (_player.Laser != null) return;

            var laser = _laserRepository.Get();
            laser.Position = _player.BulletSpawnPosition;
            laser.Rotation = _player.Rotation;
            laser.Parent = _player.LaserParent;
            laser.Scale = new Vector3(1,
                _screenService.ScreenSizeInUnits.Width + _screenService.ScreenSizeInUnits.Height);
            _player.Laser = laser;
            _player.LaserCharges--;
            _currentShootDuration = _gameplayConfig.LaserShootDuration;
            _input.Player.Fire.Disable();
        }

        private void Update()
        {
            _player.LaserCooldown -= Time.deltaTime;

            if (_player.Laser == null)
                return;

            _player.Laser.Active = true;

            _currentShootDuration -= Time.deltaTime;
            if (_currentShootDuration > 0)
                return;

            _laserRepository.Release(_player.Laser);
            _player.Laser = null;
            _input.Player.Fire.Enable();
        }

        private void Recharge()
        {
            if (_player.LaserCharges == 0 && _player.LaserCooldown <= 0)
            {
                _player.LaserCharges++;
                _player.LaserCooldown = _gameplayConfig.LaserCooldown;
            }
        }
    }
}