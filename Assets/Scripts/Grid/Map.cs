using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Tilemap ground;
        [SerializeField] private Tilemap obstacles;
        public UnityEngine.Grid grid;

        public List<GridCell<Vector2>> freeTiles = new ();
        public List<GridCell<Vector2>> blockedTiles = new ();

        private void Awake()
        {
            var cellBounds = ground.cellBounds;
            for (var x = cellBounds.min.x; x < cellBounds.max.x; x++)
            {
                for (var y = cellBounds.min.y; y < cellBounds.max.y; y++)
                {
                    var position = new Vector3Int(x, y);
                    var cellToWorld = ground.CellToWorld(position) + grid.cellSize / 2f;
                    var cell = new GridCell<Vector2>(x, y, cellToWorld);
                    if (obstacles.GetTile(position))
                    {
                        blockedTiles.Add(cell);
                    }
                    else
                    {
                        freeTiles.Add(cell);
                    }
                }
            }
        }
    }
}
