using UnityEngine;

namespace Client.Runtime.Utils
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _collider2D;

        protected Transform _transform;

        private void Awake()
        {
            _transform = transform;
            OnAwake();
        }

        public PolygonCollider2D Collider2D => _collider2D;
        public void SetPosition(Vector3 position) => _transform.position = position;
        public void SetRotation(Quaternion rotation) => _transform.localRotation = rotation;
        public void SetActive(bool state) => gameObject.SetActive(state);
        public void SetScale(Vector3 value) => _transform.localScale = value;

        protected virtual void OnAwake()
        {
        }
    }
}