using Client.Runtime.Laser;
using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Bullets
{
    public class PlayerModel : BaseModel
    {
        private PlayerView _view;
        private float _laserCooldown;
        private int _laserCharges;

        public PlayerModel(PlayerView view) : base(view)
        {
            _view = view;
        }

        public Vector2 ForwardDirection => Rotation * Vector2.up;
        public Vector2 BulletSpawnPosition => _view.BulletSpawnPosition;
        public Transform LaserParent => _view.LaserParent;
        public int Score { get; set; }

        public float LaserCooldown
        {
            get => _laserCooldown;
            set => _laserCooldown = value < 0 ? 0 : value;
        }

        public int LaserCharges
        {
            get => _laserCharges; 
            set => _laserCharges = value < 0 ? 0 : value;
        }
        public LaserModel Laser { get; set; }
    }
}