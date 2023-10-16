using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Tilemap ground;
        [SerializeField] private Tilemap obstacles;
        public Grid<GridCell<TileBase>> grid;

        private void Awake()
        {
            var size = ground.size;
            grid = new Grid<GridCell<TileBase>>(size.x, size.y, (x, y) => new GridCell<TileBase>(x, y, ground.GetTile(new Vector3Int(x, y))));
        }
    }
}
