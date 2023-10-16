using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class CaveGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap wallsTilemap;
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap ground;
        [SerializeField] private Tilemap obstacles;
        [SerializeField] private TileBase[] wallTiles;
        [SerializeField] private TileBase[] groundTiles;
        [SerializeField] private int maxWidth;
        [SerializeField] private int maxHeight;
        [SerializeField] private int cellsToRemove;

        [ContextMenu("Generate Cave")]
        public void PrintCellCoords()
        {
            print($"ground = {ground.size}");
            print($"obstacles = {obstacles.size}");
            for (var x = 0; x < ground.size.x; x++)
            {
                for (var y = 0; y < ground.size.y; y++)
                {
                    print($"{ground.GetTile(new Vector3Int(x, y))} {x} {y}");
                }
            }
        }   
    
        public void Generate()
        {
            var cave = new Cave(maxWidth, maxHeight);
            cave.DigCorridors(cellsToRemove);
        
            wallsTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
        
            foreach (var cell in cave.Grid.Cells)
            {
                if (cell.Value)
                {
                    var wall = wallTiles[Random.Range(0, wallTiles.Length)];
                    wallsTilemap.SetTile(new Vector3Int(cell.X, cell.Y, 0), wall);
                }
                else
                {
                    var ground = groundTiles[Random.Range(0, groundTiles.Length)];
                    groundTilemap.SetTile(new Vector3Int(cell.X, cell.Y, 0), ground);
                }
            }
        }

    }
}
