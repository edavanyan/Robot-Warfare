using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnemyAI;
using Loots;
using TMPro;
using UnityEngine;
using Utils.Pool;
using Random = UnityEngine.Random;

namespace Manager
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI killedText;
        
        public int limit;
        public bool LimitExceeded => limit < activeEnemies.Count;
        public List<Enemy> enemyInstances;
        public LootManager lootManager;
        private List<ComponentPool<Enemy>> enemyPools = new();

        private readonly List<Enemy> activeEnemies = new();

        private readonly Dictionary<LootType, float> lootProbabilities = new()
        {
            { LootType.Xp, 99f },
            { LootType.Sword, 1f }
        };

        private float sumOfProbs = 0;
        
        //TODO remove this, it is for cinematic 
        public LootType currentLootType = LootType.Xp;
        public int killedEnemyCount = 0;
        
        void Awake()
        {
            foreach (var enemy in enemyInstances)
            {
                enemyPools.Add(new ComponentPool<Enemy>(enemy));
            }

            foreach (var value in lootProbabilities.Values)
            {
                sumOfProbs += value;
            }
        }

        public void SetProbability(LootType type, float prob)
        {
            if (lootProbabilities.TryGetValue(type, out var probability))
            {
                sumOfProbs -= probability;
            }

            sumOfProbs += prob;
            lootProbabilities[type] = prob;
        }

        public Enemy CreateEnemy(Vector2 enemyPosition)
        {
            if (activeEnemies.Count >= limit)
            {
                return null;
            }

            var index = Random.Range(0, enemyPools.Count);
            var enemy = enemyPools[index].NewItem();
            enemy.Pool = enemyPools[index];
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
                    
                    if (currentLootType != LootType.Gold)
                    {
                        enemy.LootType = currentLootType;
                    }
                    else
                    {
                        var prob = Random.Range(0f, sumOfProbs);
                        var currentCursor = 0f;
                        foreach (var probability in lootProbabilities)
                        {
                            currentCursor += probability.Value;
                            if (prob < currentCursor)
                            {
                                enemy.LootType = probability.Key;
                                break;
                            }
                        }
                    }

                    if (killedEnemyCount < 5 || Random.value < 35f / activeEnemies.Count)
                    {
                        lootManager.DropLoot(enemy);
                    }

                    DestroyEnemy(enemy);
                    
                    killedEnemyCount++;
                }
            };
            return enemy;
        }

        private void OnGUI()
        {
            killedText.text = $"{killedEnemyCount}";
        }

        private void DestroyEnemy(Enemy enemy)
        {
            activeEnemies.Remove(enemy);
            enemy.Pool.DestroyItem(enemy);
        }
    }
}
