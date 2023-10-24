using System;
using System.Collections.Generic;
using Cameras;
using Grid;
using Loots;
using UnityEngine;
using Utils;
using Utils.Pool;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class EnemySpawner : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public new SmoothCamera2D camera;
        public Enemy enemyInstance;
        public float spawnRate;
        public int limit;
        public Map map;
        public LootManager lootManager;

        private readonly Direction[] directions =
        {
            new() { OnX = false, OnY = false },
            new() { OnX = true, OnY = false },
            new() { OnX = true, OnY = true },
            new() { OnX = false, OnY = true }
        };

        private int directionIndex = 0;

        private ComponentPool<Enemy> enemyPool;

        private readonly List<Enemy> activeEnemies = new();

        void Start()
        {
            enemyPool = new ComponentPool<Enemy>(enemyInstance);
            InvokeRepeating(nameof(Spawn), 1f, spawnRate);
        }

        void Spawn()
        {
            if (activeEnemies.Count > limit)
            {
                return;
            }
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
                var enemy = enemyPool.NewItem();
                activeEnemies.Add(enemy);
                enemy.transform.position = enemyPosition;
                enemyPosition += enemyPosition * 0.01f;
                enemy.OnDie += () =>
                {
                    if (activeEnemies.Contains(enemy))
                    {
                        activeEnemies.Remove(enemy);
                        enemyPool.DestroyItem(enemy);
                        lootManager.DropLoot(enemy);
                    }
                };
            }
        }
    }

    struct Direction
    {
        public bool OnX;
        public bool OnY;
    }
}
