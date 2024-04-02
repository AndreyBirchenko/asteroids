using Client.ControllerContainer;
using Client.Runtime.Asteroids;
using Client.Runtime.Bullets;
using Client.Runtime.UFO;
using Client.Runtime.Utils;
using Client.Runtime.Utils.Collisions;

namespace Client.Runtime.Laser
{
    public struct LaserCollisionEvt
    {
        public BaseModel Object;
    }

    public class LaserCollisionController : IRunController
    {
        private readonly LaserRepository _laserRepository = ServiceLocator<LaserRepository>.Get();
        private readonly BulletsRepository _bulletsRepository = ServiceLocator<BulletsRepository>.Get();
        private readonly AsteroidsRepository _asteroidsRepository = ServiceLocator<AsteroidsRepository>.Get();
        private readonly UfoRepository _ufoRepository = ServiceLocator<UfoRepository>.Get();
        private readonly EventBus _eventBus = ServiceLocator<EventBus>.Get();

        void IRunController.Run()
        {
            for (int i = 0; i < _laserRepository.ActiveElements.Count; i++)
            {
                var laser = _laserRepository.ActiveElements[i];
                CheckCollision(laser, _bulletsRepository);
                CheckCollision(laser, _asteroidsRepository);
                CheckCollision(laser, _ufoRepository);
            }
        }

        private void CheckCollision<T>(LaserModel laser, BaseRepository<T> repository) where T : BaseModel
        {
            var elements = repository.ActiveElements;
            for (int i = 0; i < elements.Count; i++)
            {
                if (!CollisionUtils.HasCollision(laser.Collider, elements[i].Collider))
                    continue;

                var @event = new LaserCollisionEvt() {Object = elements[i]};
                _eventBus.Publish(@event);
                repository.Release(elements[i]);
            }
        }
    }
}