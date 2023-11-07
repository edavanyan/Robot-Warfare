using System.Collections.Generic;
using Cameras;
using Grid;
using UnityEngine;
using Utils.Pool;
using CharacterController = PlayerController.CharacterController;
using Random = UnityEngine.Random;

namespace Manager
{
    public class MapManager : MonoBehaviour
    {
        public List<TerrainChunk> terrainChunks;
        private readonly float checkRadius = 0.01f;
        public LayerMask terrainMask;
        public SmoothCamera2D smoothCamera2D;

        private readonly Dictionary<Direction, Vector2> directions = new Dictionary<Direction, Vector2>()
        {
            { Direction.Left, Vector2.left },
            { Direction.Right, Vector2.right },
            { Direction.Up, Vector2.up },
            { Direction.Down, Vector2.down },
            { Direction.UpLeft, Vector2.up + Vector2.left},
            { Direction.UpRight, Vector2.up + Vector2.right},
            { Direction.DownLeft, Vector2.down + Vector2.left},
            { Direction.DownRight, Vector2.down + Vector2.right}
        };

        private GameObject current;
        private readonly List<ComponentPool<TerrainChunk>> terrainPoolList = new();
        private float chunkSize;
        private Vector2 offset;

        private void Start()
        {
            foreach (var terrainChunk in terrainChunks)
            {
                terrainPoolList.Add(new ComponentPool<TerrainChunk>(terrainChunk));
            }
            var chunk = CreateTerrainChunk(Vector2.zero);
            var position = chunk.transform.position;
            var boxCollider2D = chunk.GetComponent<BoxCollider2D>();
            chunkSize = 20;
            offset = boxCollider2D.offset;
            current = chunk.gameObject;

            foreach (var direction in directions)
            {
                CreateTerrainChunk((Vector2)position + direction.Value * chunkSize);
            }
            
            // InvokeRepeating(nameof(CheckToGenerate), 0, 0.1f);
        }

        private TerrainChunk CreateTerrainChunk(Vector2 position)
        {
            var randomIndex = Random.Range(0, terrainChunks.Count);
            var terrainChunk = terrainPoolList[randomIndex].NewItem();
            var chunk = terrainChunk.GetComponent<BoxCollider2D>();
            chunk.transform.position = position;
            terrainChunk.Owner = terrainPoolList[randomIndex];
            terrainChunk.OnTriggerEnter += CheckToGenerate;
            return terrainChunk;
        }

        void CheckToGenerate(TerrainChunk chunk)
        {
            var position = (Vector2)chunk.transform.position;
            var currentPosition = (Vector2)current.transform.position;
            if (current != chunk.gameObject)
            {
                foreach (var direction in directions)
                {
                    var terrain = Physics2D.OverlapCircle(position + offset + direction.Value * chunkSize, checkRadius, terrainMask);
                    Debug.DrawLine(position + offset,position + offset + direction.Value * chunkSize);
                    if (!terrain)
                    {
                        var terrainToDestroy = Physics2D.OverlapCircle(currentPosition + offset - direction.Value * chunkSize, checkRadius, terrainMask);
                        if (terrainToDestroy)
                        {
                            var terrainChunk = terrainToDestroy.GetComponent<TerrainChunk>();
                            terrainChunk.Owner?.DestroyItem(terrainChunk);
                        }

                        CreateTerrainChunk(position + direction.Value * chunkSize);
                    }
                }
                current = chunk.gameObject;
            }
        }

        private enum Direction
        {
            Left,
            Right,
            Down,
            Up,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }
    }
}
