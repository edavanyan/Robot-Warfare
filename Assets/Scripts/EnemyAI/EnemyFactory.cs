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
        // Start is called before the first frame update
        void Start()
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
            enemy.OnDie += () =>
            {
                if (activeEnemies.Contains(enemy))
                {
                    activeEnemies.Remove(enemy);
                    enemyPool.DestroyItem(enemy);
                    lootManager.DropLoot(enemy);
                }
            };
            return enemy;
        }
    }
}
