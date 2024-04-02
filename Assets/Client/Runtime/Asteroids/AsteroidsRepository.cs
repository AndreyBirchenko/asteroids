using System;

using Client.Runtime.Utils;

namespace Client.Runtime.Asteroids
{
    public class AsteroidsRepository : BaseRepository<AsteroidModel>
    {
        public AsteroidsRepository(Func<AsteroidModel> createFunc) : base(createFunc)
        {
        }
    }
}