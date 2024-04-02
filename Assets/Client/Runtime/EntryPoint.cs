using Client.ControllerContainer;
using Client.Runtime.Asteroids;
using Client.Runtime.Bullets;
using Client.Runtime.Config;
using Client.Runtime.EndGame;
using Client.Runtime.Laser;
using Client.Runtime.Player;
using Client.Runtime.Services;
using Client.Runtime.UFO;
using Client.Runtime.UI;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private VisualConfig _visualConfig;
        [SerializeField] private GameplayConfig _gameplayConfig;
        [SerializeField] private Canvas _mainCanvas;

        private ControllerGroup _updateControllers;
        private EventBus _eventBus;

        private void Start()
        {
            _eventBus = new EventBus();

            ServiceLocator<PlayerRepository>.Add(new PlayerRepository(() =>
                new PlayerModel(Instantiate(_visualConfig.PlayerViewPrefab))));

            ServiceLocator<BulletsRepository>.Add(new BulletsRepository(() =>
                new BulletModel(Instantiate(_visualConfig.BulletViewPrefab))));

            ServiceLocator<AsteroidsRepository>.Add(new AsteroidsRepository(() =>
                new AsteroidModel(Instantiate(_visualConfig.AsteroidViewPrefab))));

            ServiceLocator<UfoRepository>.Add(new UfoRepository(() =>
                new UfoModel(Instantiate(_visualConfig.UfoViewPrefab))));

            ServiceLocator<LaserRepository>.Add(new LaserRepository(() =>
                new LaserModel(Instantiate(_visualConfig.LaserView))));

            ServiceLocator<Input>.Add(new Input());
            ServiceLocator<VisualConfig>.Add(_visualConfig);
            ServiceLocator<GameplayConfig>.Add(_gameplayConfig);
            ServiceLocator<ScreenService>.Add(new ScreenService(Camera.main));
            ServiceLocator<EventBus>.Add(_eventBus);
            ServiceLocator<Canvas>.Add(_mainCanvas);

            _updateControllers = new ControllerGroup();
            _updateControllers
                .Add(new PlayerMovementController())
                .Add(new PlayerShootingController())
                .Add(new PlayerLaserController())
                .Add(new PlayerCollisionController())
                .Add(new BulletsCollisionController())
                .Add(new LaserCollisionController())
                .Add(new AsteroidsController())
                .Add(new UfoController())
                .Add(new PlayerScoreController())
                .Add(new PlayerInfoController())
                .Add(new EndGameController())
                .Init();
        }

        private void Update()
        {
            _updateControllers.Run();
        }

        private void OnDestroy()
        {
            _updateControllers.Dispose();
            _eventBus.Dispose();
        }
    }
}