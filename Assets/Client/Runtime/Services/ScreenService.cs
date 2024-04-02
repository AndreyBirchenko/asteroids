using Client.Runtime.Utils;

using UnityEngine;

namespace Client.Runtime.Services
{
    public class ScreenService
    {
        private readonly Camera _camera;

        public ScreenService(Camera camera)
        {
            _camera = camera;
            MaxBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            MinBounds = _camera.ScreenToWorldPoint(Vector3.zero);

            SideMap = new[]
            {
                new Vector2(MinBounds.x, 0),
                new Vector2(MaxBounds.x, 0),
                new Vector2(0, MinBounds.y),
                new Vector2(0, MaxBounds.y)
            };

            ScreenSizeInUnits = GetScreenSizeInUnits();
        }

        private Vector2[] SideMap { get; }

        public Vector2 MinBounds { get; }

        public Vector2 MaxBounds { get; }
        public (float Width, float Height) ScreenSizeInUnits { get; }

        public bool IsOutOfScreen(Vector2 position)
        {
            return position.x > MaxBounds.x ||
                   position.y > MaxBounds.y ||
                   position.x < MinBounds.x ||
                   position.y < MinBounds.y;
        }

        public Vector2 GetRandomPositionOnScreenSide()
        {
            var side = SideMap.GetRandomElement();
            return side.x == 0
                ? new Vector2(Random.Range(MinBounds.x, MaxBounds.x), side.y)
                : new Vector2(side.x, Random.Range(MinBounds.y, MaxBounds.y));
        }

        private (float Width, float Height) GetScreenSizeInUnits()
        {
            var leftBottom = _camera.ScreenToWorldPoint(new Vector3(0, 0));
            var rightBottom = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0));
            var leftTop = _camera.ScreenToWorldPoint(new Vector3(0, Screen.height));
            var width = (rightBottom - leftBottom).magnitude;
            var height = (leftTop - leftBottom).magnitude;
            return (width, height);
        }
    }
}