using Client.ControllerContainer;
using Client.Runtime.Config;
using Client.Runtime.Services;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Bullets
{
    public sealed class PlayerMovementController : IInitController, IRunController
    {
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();
        private readonly Input _input = ServiceLocator<Input>.Get();
        private readonly ScreenService _screenService = ServiceLocator<ScreenService>.Get();

        private PlayerModel _player;

        void IInitController.Init()
        {
            _input.Enable();

            _player = _playerRepository.Player;
            _player.Position = Vector3.zero;
        }

        void IRunController.Run()
        {
            UpdateRotation();
            UpdatePosition();
            HoldOnScreen();
        }

        private void UpdateRotation()
        {
            var rotationInput = _input.Player.Movement.ReadValue<Vector2>().x;
            if (rotationInput == 0)
                return;

            var angle = _gameplayConfig.RotationSpeed * rotationInput * Time.deltaTime * -1;
            _player.Rotate(angle);
        }

        private void UpdatePosition()
        {
            var input = _input.Player.Movement.ReadValue<Vector2>().y;

            if (_player.Velocity != Vector2.zero)
            {
                var deceleration = _gameplayConfig.Deceleration * Time.deltaTime;
                var currentX = _player.Velocity.x;
                var currentY = _player.Velocity.y;
                var newX = currentX > 0 ? currentX - deceleration : currentX + deceleration;
                var newY = currentY > 0 ? currentY - deceleration : currentY + deceleration;
                _player.Velocity = new Vector2(newX, newY);
            }

            _player.Velocity += input * _gameplayConfig.Acceleration * _player.ForwardDirection * Time.deltaTime;
            _player.Position += _player.Velocity * Time.deltaTime;
        }

        private void HoldOnScreen()
        {
            var max = _screenService.MaxBounds;
            var min = _screenService.MinBounds;
            var position = _player.Position;
            if (position.x > max.x) _player.Position = new Vector2(min.x, _player.Position.y);
            if (position.x < min.x) _player.Position = new Vector2(max.x, _player.Position.y);
            if (position.y > max.y) _player.Position = new Vector2(_player.Position.x, min.y);
            if (position.y < min.y) _player.Position = new Vector2(_player.Position.x, max.y);
        }
    }
}