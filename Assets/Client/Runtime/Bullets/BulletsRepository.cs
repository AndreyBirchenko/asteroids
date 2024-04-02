using System;

using Client.Runtime.Utils;

namespace Client.Runtime.Bullets
{
    public class BulletsRepository : BaseRepository<BulletModel>
    {
        public BulletsRepository(Func<BulletModel> createFunc) : base(createFunc)
        {
        }
    }
}