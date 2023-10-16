using System.Linq;
using UnityEngine;
using Utils;

namespace Grid
{
    public class Cave
    {
        private readonly int maxWidth;
        private readonly int maxHeight;

        public Grid<GridCell<bool>> Grid { get; private set; }

        public Cave(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            Grid = new Grid<GridCell<bool>>(maxWidth, maxHeight, InitCaveCell);
        }

        private GridCell<bool> InitCaveCell(int x, int y)
        {
            return new GridCell<bool>(x, y, true);
        }

        public void DigCorridors(int cellsToRemove)
        {
            var walkerPosition = new Vector2Int(maxWidth / 2, maxHeight / 2);
            while (cellsToRemove > 0)
            {
                var direction = RandomUtils.GetRandomValue<Direction>();
                var newPos = walkerPosition + direction.ToCoords();
                if (!Grid.IsValid(newPos, true)) continue;
            
                var gridCell = Grid.Get(newPos);
                if (gridCell.Value)
                {
                    gridCell.Value = false;
                    cellsToRemove--;
                }

                walkerPosition = newPos;
            }
            Shrink();
        }

        public void Shrink()
        {
            var emptyCells = Grid.Cells.Where(c => !c.Value).ToArray();
            var minX = emptyCells.Min(c => c.X);
            var maxX = emptyCells.Max(c => c.X);
            var shrankWidth = maxX - minX + 3;
            var minY = emptyCells.Min(c => c.Y);
            var maxY = emptyCells.Max(c => c.Y);
            var shrankHeight = maxY - minY + 3;

            var newGrid = new Grid<GridCell<bool>>(shrankWidth, shrankHeight, (x, y) => new GridCell<bool>(x, y, true));
            for (var x = minX - 1; x <= maxX; x++)
            {
                for (var y = minY - 1; y <= maxY; y++)
                {
                    var value = Grid.Get(x, y).Value;
                    newGrid.Get(x - minX + 1, y - minY + 1).Value = value;
                }
            }

            Grid = newGrid;
        }
    }
}
