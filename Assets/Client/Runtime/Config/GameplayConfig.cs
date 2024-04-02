using UnityEngine;

namespace Client.Runtime.Config
{
    [CreateAssetMenu(fileName = nameof(GameplayConfig), menuName = "Client/" + nameof(GameplayConfig), order = 0)]
    public class GameplayConfig : ScriptableObject
    {
        [Header("Player")]
        [Min(0)] public float Deceleration;
        [Min(0)] public float Acceleration;
        public float RotationSpeed = 1;
        
        [Header("Bullet")]
        public float BulletSpeed;

        [Header("Asteroids")] 
        public Vector2 AsteroidsSpawnRate;
        public Vector2 AsteroidsVelocityRange;
        public float AsteroidsSpawnAngleOffset;
        public float BigAsteroidRotationSpeed;
        public float SmallAsteroidRotationSpeed;
        public Vector2Int ShardsSpawnRange;
        [Range(0, 1)] public float ShardsSpawnChance;

        [Header("UFO")] 
        public Vector2 UfoSpawnRate;
        public float UfoMoveSpeed;
        public float UfoShootingInterval;

        [Header("Score")] 
        public int ScoreForAsteroid;
        public int ScoreForUfo;

        [Header("Laser")] 
        public float LaserCooldown;
        public float LaserShootDuration;
    }
}