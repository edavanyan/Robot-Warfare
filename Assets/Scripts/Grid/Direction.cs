using System;
using UnityEngine;

namespace Grid
{
    public enum Direction
    {
        North, South, East, West
    }

    public static class DirectionExtensions
    {
        public static Vector2Int ToCoords(this Direction self)
        {
            if (self == Direction.North)
            {
                return new Vector2Int(0, 1);
            }
            if (self == Direction.South)
            {
                return new Vector2Int(0, -1);
            }
            if (self == Direction.East)
            {
                return new Vector2Int(1, 0);
            }
            if (self == Direction.West)
            {
                return new Vector2Int(-1, 0);
            }
            throw new ArgumentOutOfRangeException();
        }

        public static Direction ToDirection(this Vector2Int self)
        {
            if (self == new Vector2Int(0, 1)) return Direction.North;
            if (self == new Vector2Int(0, -1)) return Direction.South;
            if (self == new Vector2Int(1, 0)) return Direction.East;
            if (self == new Vector2Int(-1, 0)) return Direction.West;
            throw new ArgumentException();
        }
    }
}