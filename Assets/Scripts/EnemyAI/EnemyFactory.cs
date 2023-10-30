using System;
using System.Collections.Generic;
using Loots;
using UnityEngine;
using Utils.Pool;

namespace EnemyAI
{
    public class EnemyFactory : MonoBehaviour
    {
        public int limit;
        public Enemy enemyInstance;
        public LootManager lootManager;
        private ComponentPool<Enemy> enemyPool;

        private readonly List<Enemy> activeEnemies = new();
        void Awake()
        {
            enemyPool = new ComponentPool<Enemy>(enemyInstance);
        }

        public Enemy CreateEnemy(Vector2 enemyPosition)
        {
            if (activeEnemies.Count >= limit)
            {
                return null;
            }
            var enemy = enemyPool.NewItem();
            activeEnemies.Add(enemy);
            enemy.transform.position = enemyPosition;
            enemy.OnFaraway += () =>
            {
                if (activeEnemies.Count >= 30 && 
                    activeEnemies.Contains(enemy))
                {
                    DestroyEnemy(enemy);
                }
            };
            enemy.OnDie += () =>
            {
                if (activeEnemies.Contains(enemy))
                {
                    DestroyEnemy(enemy);
                    lootManager.DropLoot(enemy);
                }
            };
            return enemy;
        }

        private void DestroyEnemy(Enemy enemy)
        {
            activeEnemies.Remove(enemy);
            enemyPool.DestroyItem(enemy);
        }
    }
}
