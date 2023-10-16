using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class Grid<T>
    {
        public T[] Cells { get; }
        public int Width { get; }
        public int Height { get; }

        public delegate T InitFunction(int x, int y);
    
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            Cells = new T[width * height];
        }

        public Grid(int width, int height, InitFunction init) : this(width, height)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Set(x, y, init(x, y));
                }
            }
        }

        private int CoordsToIndex(int x, int y)
        {
            return y * Width + x;
        }

        private Vector2Int IndexToCoords(int index)
        {
            return new Vector2Int(index % Width, index / Width);
        }

        public void Set(int x, int y, T value)
        {
            Cells[CoordsToIndex(x, y)] = value;
        }

        public void Set(Vector2Int coords, T value)
        {
            Set(coords.x, coords.y, value);
        }

        public T Get(int x, int y)
        {
            return Cells[CoordsToIndex(x, y)];
        }

        public T Get(Vector2Int coords)
        {
            return Get(coords.x, coords.y);
        }

        public bool IsValid(int x, int y, bool safeWalls = false)
        {
            return safeWalls ? x > 0 && x < Width - 1 && y > 0 && y < Height - 1 :
                x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsValid(Vector2Int coords, bool safeWalls = false)
        {
            return IsValid(coords.x, coords.y, safeWalls);
        }

        public Vector2Int GetCoords(T value)
        {
            var i = Array.IndexOf(Cells, value);
            if (i == -1)
            {
                throw new ArgumentException();
            }

            return IndexToCoords(i);
        }

        public List<T> GetNeighbours(Vector2Int coords, bool safeWalls = false)
        {
            var neighbours = new List<T>();
            var directions = (Direction[])Enum.GetValues(typeof(Direction));
            foreach (var direction in directions)
            {
                var neighbourCoords = coords + direction.ToCoords();
                if (IsValid(neighbourCoords, safeWalls))
                {
                    neighbours.Add(Get(neighbourCoords));
                }
            }

            return neighbours;
        }
    }
}
 