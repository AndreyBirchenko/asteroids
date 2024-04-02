using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Laser
{
    public class LaserView : BaseView
    {
        public void SetParent(Transform parent)
        {
            _transform.SetParent(parent);
        }
    }
}