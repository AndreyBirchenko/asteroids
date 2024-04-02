using System;

using Client.Runtime.Utils;

namespace Client.Runtime.Laser
{
    public class LaserRepository : BaseRepository<LaserModel>
    {
        public LaserRepository(Func<LaserModel> createFunc) : base(createFunc)
        {
        }

        protected override void OnRelease(LaserModel model)
        {
            model.Parent = null;
        }
    }
}