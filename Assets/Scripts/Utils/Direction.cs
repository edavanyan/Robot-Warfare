using UnityEngine;

namespace Utils
{
    public class Direction
    {
        public Vector2 DirectionVector { get; private set; }
        public Direction MirrorDirection { get; private set; }
        public Direction[] Neighbors { get; private set; }
        
        private Direction(Vector2 dir, Direction mir, Direction[] neighbors)
        {
            DirectionVector = dir;
            MirrorDirection = mir;
            Neighbors = neighbors;
        }
        
        public static Direction Left { get; private set; }
        public static Direction Right { get; private set; }
        public static Direction Up { get; private set; }
        public static Direction Down { get; private set; }
        public static Direction UpLeft { get; private set; }
        public static Direction UpRight { get; private set; }
        public static Direction DownLeft { get; private set; }
        public static Direction DownRight { get; private set; }

        private static class DirectionCreator
        {
            static DirectionCreator()
            {
                Left = new Direction(Vector2.left, Right, new []{UpRight, DownRight});
                Right = new Direction(Vector2.right, Left, new []{UpLeft, DownLeft});
                Up = new Direction(Vector2.up, Down, new []{DownRight, DownLeft});
                Down = new Direction(Vector2.down, Up, new []{UpRight, UpLeft});
                UpLeft = new Direction(Vector2.up + Vector2.left, DownRight, null);
                UpRight = new Direction(Vector2.up + Vector2.right, DownLeft, null);
                DownLeft = new Direction(Vector2.down + Vector2.left, UpRight, null);
                DownRight = new Direction(Vector2.down + Vector2.right, DownRight, null);
            }
        }
    }
}
