using System.Collections.Generic;

using Client.ControllerContainer;
using Client.Runtime.Asteroids;
using Client.Runtime.UFO;
using Client.Runtime.Utils;
using Client.Runtime.Utils.Collisions;

namespace Client.Runtime.Bullets
{
    public struct PlayerCollisionEvt
    {
    }

    public class PlayerCollisionController : IRunController
    {
        private readonly AsteroidsRepository _asteroidsRepository = ServiceLocator<AsteroidsRepository>.Get();
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly UfoRepository _ufoRepository = ServiceLocator<UfoRepository>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();


        public void Run()
        {
            CheckCollision(_asteroidsRepository.ActiveElements);
            CheckCollision(_ufoRepository.ActiveElements);
        }
        
        private void CheckCollision<T>(List<T> elements) where T : BaseModel
        {
            var player = _playerRepository.Player;
            
            for (int i = 0; i < elements.Count; i++)
            {
                if(!CollisionUtils.HasCollision(player.Collider, elements[i].Collider))
                    continue;

                _playerRepository.Release(player);

                var @event = new PlayerCollisionEvt();
                _eventBus.Publish(@event);
            }
        }
    }
}