using UnityEngine;

namespace Client.Runtime.Utils
{
    public class BaseModel
    {
        private BaseView _view;
        private Vector3 _position;
        private bool _active;
        private Quaternion _rotation = Quaternion.identity;
        private Vector3 _scale;

        public BaseModel(BaseView view)
        {
            _view = view;
        }

        public PolygonCollider2D Collider => _view.Collider2D;

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                _view.SetActive(_active);
            }
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _view.SetPosition(_position);
            }
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _view.SetRotation(_rotation);
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _view.SetScale(_scale);
            }
        }

        public Vector2 Velocity { get; set; }


        public void Rotate(float angle)
        {
            Rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}