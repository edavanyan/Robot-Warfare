using UnityEngine;
using UnityEngine.Tilemaps;

namespace ItemSystem
{
    public class TilemapLayer : MonoBehaviour
    {
        protected Tilemap Tilemap { get; private set; }

        protected void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
        }
    }
}
