using Client.Runtime.Asteroids;
using Client.Runtime.Bullets;
using Client.Runtime.EndGame;
using Client.Runtime.Laser;
using Client.Runtime.UFO;
using Client.Runtime.UI;

using UnityEngine;

namespace Client.Runtime.Config
{
    [CreateAssetMenu(fileName = nameof(VisualConfig), menuName = "Client/" + nameof(VisualConfig), order = 0)]
    public class VisualConfig : ScriptableObject
    {
        public PlayerView PlayerViewPrefab;
        public BulletView BulletViewPrefab;
        public AsteroidView AsteroidViewPrefab;
        public UfoView UfoViewPrefab;
        public PlayerInfoView PlayerInfoViewPrefab;
        public EndGameView EndGameViewPrefab;
        public LaserView LaserView;
    }
}