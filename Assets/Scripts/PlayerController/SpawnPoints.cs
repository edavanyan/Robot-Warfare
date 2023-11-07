using System;
using System.Collections.Generic;
using Cameras;
using UnityEngine;
using Utils;

namespace PlayerController
{
    public class SpawnPoints : MonoBehaviour
    {
        private readonly List<Vector2> directions = new();
        private Rect rect;

        void Awake()
        {
            var camera2D = transform.parent.GetComponent<SmoothCamera2D>();
            var boundsMin = camera2D.CameraBoundsMin;

            var cameraSize = new Vector2(camera2D.cameraBounds.x + 1f, camera2D.cameraBounds.y + 1f);
            var position = (Vector2)transform.position;
            rect = new Rect(position - cameraSize, cameraSize * 2);
            var rectPoints = new[]
            {
                rect.min,
                new(rect.xMin, rect.yMax),
                rect.max,
                new(rect.xMax, rect.yMin),
                rect.min
            };

            for (var i = 0; i < transform.childCount; i++)
            {
                var spawners = transform.GetChild(i);
                var deltaAngle = 360f / spawners.childCount;
                var direction = (boundsMin - position) * 2;
                for (var index = 0; index < spawners.childCount; index++)
                {
                    if (index > 0 || i > 0)
                    {
                        direction = Quaternion.Euler(0, 0, deltaAngle) * direction;
                    }

                    for (var j = 0; j < rectPoints.Length - 1; j++)
                    {
                        {
                            if (Intersects(position,
                                    position + direction,
                                    rectPoints[j],
                                    rectPoints[j + 1],
                                    out var point))
                            {
                                spawners.GetChild(index).position = point;
                            }
                        }
                    }
                }
            }
        }

        private bool Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            float bDotDPerp = b.x * d.y - b.y * d.x;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            float t = (c.x * d.y - c.y * d.x) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.x * b.y - c.y * b.x) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            intersection = a1 + t * b;

            return true;
        }
    }
}
