using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Asteroids
{
    public class AsteroidModel : BaseModel
    {
        private AsteroidView _view;
        private AsteroidSize _size;

        public AsteroidModel(AsteroidView view) : base(view)
        {
            _view = view;
        }

        public AsteroidSize Size
        {
            get => _size;
            set
            {
                _size = value;
                _view.SetScale(_size == AsteroidSize.Big ? Vector3.one * 2 : Vector3.one);
            }
        }
    }
}