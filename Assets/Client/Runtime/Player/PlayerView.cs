using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Bullets
{
    public class PlayerView : BaseView
    {
        [SerializeField] private Transform _bulletSpawnPoint;

        public Vector2 BulletSpawnPosition => _bulletSpawnPoint.position;
        public Transform LaserParent => _bulletSpawnPoint;
    }
}