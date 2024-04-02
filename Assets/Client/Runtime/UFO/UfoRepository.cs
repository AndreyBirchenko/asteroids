using System;

using Client.Runtime.Utils;

namespace Client.Runtime.UFO
{
    public class UfoRepository : BaseRepository<UfoModel>
    {
        public UfoRepository(Func<UfoModel> createFunc) : base(createFunc)
        {
        }
    }
}