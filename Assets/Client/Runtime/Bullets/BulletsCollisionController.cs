using System.Collections.Generic;

using Client.ControllerContainer;
using Client.Runtime.Asteroids;
using Client.Runtime.UFO;
using Client.Runtime.Utils;
using Client.Runtime.Utils.Collisions;

namespace Client.Runtime.Bullets
{
    public struct BulletsCollisionEvt
    {
        public BulletModel Bullet;
        public BaseModel Object;
    }
    
    public class BulletsCollisionController : IRunController
    {
        private readonly BulletsRepository _bulletsRepository = ServiceLocator<BulletsRepository>.Get();
        private readonly AsteroidsRepository _asteroidsRepository = ServiceLocator<AsteroidsRepository>.Get();
        private readonly PlayerRepository _playerRepository = ServiceLocator<PlayerRepository>.Get();
        private readonly UfoRepository _ufoRepository = ServiceLocator<UfoRepository>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();
        
        void IRunController.Run()
        {
            for (int i = 0; i < _bulletsRepository.ActiveElements.Count; i++)
            {
                var bullet = _bulletsRepository.ActiveElements[i];
                CheckCollision(bullet, _asteroidsRepository.ActiveElements);
                CheckCollision(bullet, _playerRepository.ActiveElements);
                CheckCollision(bullet, _ufoRepository.ActiveElements);
            }
        }

        private void CheckCollision<T>(BulletModel bullet, List<T> elements) where T : BaseModel
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if(!CollisionUtils.HasCollision(bullet.Collider, elements[i].Collider))
                    continue;

                _bulletsRepository.Release(bullet);

                var @event = new BulletsCollisionEvt() {Object = elements[i], Bullet = bullet};
                _eventBus.Publish(@event);
            }
        }
    }
}