using System;

using Client.Runtime.Utils;

namespace Client.Runtime.Bullets
{
    public class PlayerRepository : BaseRepository<PlayerModel>
    {
        private PlayerModel _player;

        public PlayerRepository(Func<PlayerModel> createFunc) : base(createFunc)
        {
        }

        public PlayerModel Player => _player ?? Create();

        private PlayerModel Create()
        {
            _player = Get();
            return _player;
        }
    }
}