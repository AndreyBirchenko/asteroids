using System.Collections.Generic;

using UnityEngine;

using Vector2 = UnityEngine.Vector2;

namespace Client.Runtime.Utils.Collisions
{
    public class CollisionUtils
    {
        private static readonly List<Vector2> _pointsCacheA = new();
        private static readonly List<Vector2> _pointsCacheB = new();
        private static readonly List<Vector2> _edgeCacheA = new();
        private static readonly List<Vector2> _edgeCacheB = new();

        public static bool HasCollision(PolygonCollider2D colliderA, PolygonCollider2D colliderB)
        {
            var polygonA = new Polygon(GetWorldPoints(colliderA, _pointsCacheA), _edgeCacheA);
            var polygonB = new Polygon(GetWorldPoints(colliderB, _pointsCacheB), _edgeCacheB);
            int edgeCountA = polygonA.Edges.Count;
            int edgeCountB = polygonB.Edges.Count;
            Vector2 edge;

            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                edge = edgeIndex < edgeCountA ? polygonA.Edges[edgeIndex] : polygonB.Edges[edgeIndex - edgeCountA];

                Vector2 axis = new Vector2(-edge.y, edge.x).normalized;

                float minA = 0;
                float minB = 0;
                float maxA = 0;
                float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                if (GetDistanceInterval(minA, maxA, minB, maxB) > 0)
                    return false;
            }

            return true;
        }

        private static float GetDistanceInterval(float minA, float maxA, float minB, float maxB)
        {
            return minA < minB ? minB - maxA : minA - maxB;
        }

        private static List<Vector2> GetWorldPoints(PolygonCollider2D collider, List<Vector2> cache)
        {
            cache.Clear();
            for (int i = 0; i < collider.points.Length; i++)
            {
                var point = collider.transform.TransformPoint(collider.points[i]);
                cache.Add(point);
            }

            return cache;
        }

        private static void ProjectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
        {
            var d = Vector2.Dot(axis, polygon.Points[0]);
            min = d;
            max = d;
            for (var i = 0; i < polygon.Points.Count; i++)
            {
                d = Vector2.Dot(polygon.Points[i], axis);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }
    }

    public struct Polygon
    {
        private List<Vector2> _points;
        private List<Vector2> _edges;

        public Polygon(List<Vector2> points, List<Vector2> edges)
        {
            _points = points;
            _edges = edges;
            BuildEdges();
        }

        public List<Vector2> Edges => _edges;

        public List<Vector2> Points => _points;

        private void BuildEdges()
        {
            Vector2 p1;
            Vector2 p2;
            _edges.Clear();
            for (int i = 0; i < _points.Count; i++)
            {
                p1 = _points[i];
                if (i + 1 >= _points.Count)
                {
                    p2 = _points[0];
                }
                else
                {
                    p2 = _points[i + 1];
                }

                _edges.Add(p2 - p1);
            }
        }
    }
}