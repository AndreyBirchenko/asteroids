using TMPro;

using UnityEngine;

namespace Client.Runtime.UI
{
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coordinatesTmp;
        [SerializeField] private TMP_Text _angleTmp;
        [SerializeField] private TMP_Text _velocityTmp;
        [SerializeField] private TMP_Text _laserChargesTmp;
        [SerializeField] private TMP_Text _laserCooldownTmp;

        public void SetCoordinates(Vector2 coordinates)
        {
            _coordinatesTmp.text = $"XY: {coordinates}";
        }

        public void SetAngle(float angle)
        {
            _angleTmp.text = $"Angle: {angle:F2}";
        }

        public void SetVelocity(Vector2 velocity)
        {
            _velocityTmp.text = $"Velocity: {velocity}";
        }

        public void SetLaserCharges(int count)
        {
            _laserChargesTmp.text = $"Laser Charges: {count}";
        }

        public void SetLaserCooldown(float cooldown)
        {
            _laserCooldownTmp.text = $"Laser Cooldown: {cooldown:F1}";
        }
    }
}