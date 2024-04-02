using Client.ControllerContainer;
using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.UI
{
    public class PlayerInfoController : IInitController, IRunController
    {
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly VisualConfig _visualConfig = ServiceLocator<VisualConfig>.Get();
        private readonly Canvas _mainCanvas = ServiceLocator<Canvas>.Get();
        private PlayerInfoView _playerInfoView;
        private PlayerModel _player;

        void IInitController.Init()
        {
            _playerInfoView = Object.Instantiate(_visualConfig.PlayerInfoViewPrefab, _mainCanvas.transform);
            _player = _playerRepository.Player;
        }

        void IRunController.Run()
        {
            _playerInfoView.SetCoordinates(_player.Position);
            _playerInfoView.SetAngle(_player.Rotation.eulerAngles.z);
            _playerInfoView.SetVelocity(_player.Velocity);
            _playerInfoView.SetLaserCooldown(_player.LaserCooldown);
            _playerInfoView.SetLaserCharges(_player.LaserCharges);
        }
    }
}