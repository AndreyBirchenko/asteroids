using System;

using Client.ControllerContainer;
using Client.Runtime.Asteroids;
using Client.Runtime.Config;
using Client.Runtime.Laser;
using Client.Runtime.UFO;
using Client.Runtime.Utils;

namespace Client.Runtime.Bullets
{
    public class PlayerScoreController : IInitController
    {
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly GameplayConfig _gameplayConfig = ServiceLocator<GameplayConfig>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();

        public void Init()
        {
            _eventBus.Subscribe<BulletsCollisionEvt>(HandleBulletCollision);
            _eventBus.Subscribe<LaserCollisionEvt>(HandleLaserCollision);
        }

        private void HandleLaserCollision(LaserCollisionEvt @event)
        {
            if (@event.Object is BulletModel)
                return;

            var score = GetScore(@event.Object);
            _playerRepository.Player.Score += score;
        }

        private void HandleBulletCollision(BulletsCollisionEvt @event)
        {
            if (@event.Bullet.Shooter is not PlayerModel player)
                return;

            var score = GetScore(@event.Object);
            player.Score += score;
        }

        private int GetScore(BaseModel element)
        {
            switch (element)
            {
                case AsteroidModel:
                    return _gameplayConfig.ScoreForAsteroid;
                case UfoModel:
                    return _gameplayConfig.ScoreForUfo;
            }

            throw new ArgumentException();
        }
    }
}