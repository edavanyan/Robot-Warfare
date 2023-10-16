using UnityEngine;

namespace ItemSystem
{
    public class ConstructionLayer : TilemapLayer
    {
        public void Build(Vector3 worldCoords, BuildableItem item)
        {
            var coords = Tilemap.WorldToCell(worldCoords);
            if (item.Tile != null)
            {
                Tilemap.SetTile(coords, item.Tile);
            }
        }
    }
}
