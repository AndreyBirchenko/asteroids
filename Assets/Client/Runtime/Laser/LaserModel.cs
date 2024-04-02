using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Laser
{
    public class LaserModel : BaseModel
    {
        private Transform _parent;
        private LaserView _view;
        
        public LaserModel(LaserView view) : base(view)
        {
            _view = view;
        }

        public Transform Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                _view.SetParent(_parent);
            }
        }
    }
}