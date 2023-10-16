using UnityEngine;
using UnityEngine.Tilemaps;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Item/New Buildable Item")]
    public class BuildableItem : ScriptableObject
    {
        [field:SerializeField]
        public string Name { get; private set; }
        [field:SerializeField]
        public TileBase Tile { get; private set; }
    }
}
