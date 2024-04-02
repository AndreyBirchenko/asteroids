using Client.ControllerContainer;
using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.Utils;

using UnityEngine;
using UnityEngine.SceneManagement;

using Object = UnityEngine.Object;

namespace Client.Runtime.EndGame
{
    public class EndGameController : IInitController
    {
        private readonly VisualConfig _visualConfig = ServiceLocator<VisualConfig>.Get();
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly Canvas _mainCanvas = ServiceLocator<Canvas>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();
        private readonly Input _input = ServiceLocator<Input>.Get();
        private EndGameView _endGameView;

        void IInitController.Init()
        {
            _endGameView = Object.Instantiate(_visualConfig.EndGameViewPrefab, _mainCanvas.transform);
            _endGameView.SetActive(false);
            _eventBus.Subscribe<PlayerCollisionEvt>(HandlePlayerCollision);
            _eventBus.Subscribe<BulletsCollisionEvt>(HandleBulletCollision);
        }

        private void HandlePlayerCollision(PlayerCollisionEvt @event)
        {
            ShowEndGame();
        }

        private void HandleBulletCollision(BulletsCollisionEvt @event)
        {
            if (@event.Object is PlayerModel)
            {
                _playerRepository.Player.Active = false;
                ShowEndGame();
            }
        }


        private void ShowEndGame()
        {
            _endGameView.SetActive(true);
            _endGameView.SetScore(_playerRepository.Player.Score);
            _endGameView.RestartButton.onClick.RemoveAllListeners();
            _endGameView.RestartButton.onClick.AddListener(RestartGame);
            _input.Disable();
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}