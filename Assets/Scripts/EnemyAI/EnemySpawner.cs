using System.Collections;
using Cameras;
using Grid;
using Manager;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class EnemySpawner : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public new SmoothCamera2D camera;
        public float spawnRate;
        public float delay;
        public int limit;
        public Map map;
        public BoxCollider2D spawnArea;
        public int maxSpawnCount;

        private readonly Direction[] directions =
        {
            new() { OnX = false, OnY = false },
            new() { OnX = true, OnY = false },
            new() { OnX = true, OnY = true },
            new() { OnX = false, OnY = true }
        };
        private int directionIndex = 0;
        private EnemyManager enemyManager;
        private int spawnCount;


        void Start()
        {
            enemyManager = API.EnemyManager;
            if (spawnRate > 0)
            {
                InvokeRepeating(nameof(SpawnAtArea), delay, spawnRate);
            }
            else
            {
                spawnCount = limit;
                Invoke(nameof(SpawnAtPosition), delay);
            }
        }

        void Spawn()
        {
            var enemyPosition = Constants.Nan;
            while (enemyPosition == Constants.Nan)
            {
                var boundsMax = camera.CameraBoundsMax;
                var boundsMin = camera.CameraBoundsMin;
                var randomAxis = directions[directionIndex].OnX;
                var isMax = directions[directionIndex].OnY;
                var spawnPointX = randomAxis ? Random.Range(boundsMin.x, boundsMax.x) :
                    isMax ? boundsMax.x + 0.5f : boundsMin.x - 0.5f;
                var spawnPointY = randomAxis
                    ? isMax ? boundsMax.y + 0.5f : boundsMin.y - 0.5f
                    : Random.Range(boundsMin.y, boundsMax.y);
                enemyPosition = map.GetFreeTileAt(spawnPointX, spawnPointY);

                directionIndex++;
                if (directionIndex >= directions.Length)
                {
                    directionIndex = 0;
                } 
            }

            var spawnCount = Random.Range(3, 10);
            for (var i = 0; i < spawnCount; i++)
            {
                enemyManager.CreateEnemy(enemyPosition);
                enemyPosition += enemyPosition * 0.01f;
            }
        }

        void SpawnAtPosition()
        {
            SpawnAtPosition(spawnCount);
        }

        void SpawnAtPosition(int count)
        {
            var range = count > 0 ? count : Random.Range(3, 10);
            for (var i = 0; i < range; i++)
            {
                enemyManager.CreateEnemy(transform.position);
            }
        }

        void SpawnAtArea()
        {
            if (enemyManager.LimitExceeded)
            {
                return;
            }
            var count = Random.Range(1, maxSpawnCount + 1);
            StartCoroutine(nameof(SpawnAtAreaRoutine), count);
        }

        IEnumerator SpawnAtAreaRoutine(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var position = new Vector2();
                var spawnAreaBounds = spawnArea.bounds;
                position.x = Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x);
                position.y = Random.Range(spawnAreaBounds.min.y, spawnAreaBounds.max.y);
                enemyManager.CreateEnemy(position);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    struct Direction
    {
        public bool OnX;
        public bool OnY;
    }
}
