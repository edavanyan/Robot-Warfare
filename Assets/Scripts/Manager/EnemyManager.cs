using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Attack;
using DG.Tweening;
using EnemyAI;
using Loots;
using PlayerController;
using TMPro;
using UnityEngine;
using Utils.Pool;
using Random = UnityEngine.Random;

namespace Manager
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI killedText;
        [SerializeField] private FloatingNumber floatingNumberProto;
        
        public int limit;
        public bool LimitExceeded => limit < activeEnemies.Count;
        public List<Enemy> enemyInstances;
        public LootManager lootManager;
        private readonly List<ComponentPool<Enemy>> enemyPools = new();
        private ComponentPool<FloatingNumber> floatingNumberPool;

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

        private int waveIndex;

        public int EnemyDifficultyIndex
        {
            get => waveIndex;
            set => waveIndex = value >= enemyPools.Count ? 0 : value;
        }

        void Awake()
        {
            foreach (var enemy in enemyInstances)
            {
                enemyPools.Add(new ComponentPool<Enemy>(enemy));
            }

            floatingNumberPool = new ComponentPool<FloatingNumber>(floatingNumberProto);

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

            var index = EnemyDifficultyIndex;
            var enemy = enemyPools[index].NewItem();
            enemy.Pool = enemyPools[index];
            activeEnemies.Add(enemy);
            enemy.transform.position = enemyPosition;
            if (!enemy.OnFarAwaySubscribed())
            {
                enemy.SubscribeOnFarAway(() =>
                {
                    if (activeEnemies.Count >= 30 &&
                        activeEnemies.Contains(enemy))
                    {
                        DestroyEnemy(enemy);
                    }
                });
            }

            if (!enemy.OnHitSubscribed())
            {
                enemy.SubscribeOnHit(number =>
                {
                    var floatingNumber = floatingNumberPool.NewItem();
                    floatingNumber.transform.SetParent(API.UIManager.transform);
                    floatingNumber.Number = number;
                    
                    var position = enemy.transform.position;
                    floatingNumber.RectTransform.anchoredPosition = API.UIManager.ScreenToCanvasPosition(Camera.main.WorldToScreenPoint(position));
                    DOTween.Sequence()
                        .Append(floatingNumber.RectTransform.DOScale(Vector3.one, 0.05f))
                        .Join(DOTween.To(() => position, pos => position = pos, new Vector3(0, 0.5f, 0), 0.4f).SetRelative(true))
                        .Append(floatingNumber.RectTransform.DOScale(Vector3.zero, 0.1f))
                        .OnUpdate(() =>
                        {
                            floatingNumber.RectTransform.anchoredPosition = API.UIManager.ScreenToCanvasPosition(Camera.main.WorldToScreenPoint(position));
                        })
                        .AppendCallback(() =>
                        {
                            floatingNumberPool.DestroyItem(floatingNumber);
                        });
                });
            }

            if (!enemy.OnDieSubscribed())
            {
                enemy.SubscribeOnDie(() =>
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
                        lootManager.DropLoot(enemy);

                        DestroyEnemy(enemy);

                        killedEnemyCount++;
                    }
                });
            }

            return enemy;
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].Act();
            }
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
