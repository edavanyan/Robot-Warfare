using System;
using System.Collections.Generic;
using EnemyAI;
using Loots;
using PlayerController;
using UnityEngine;
using Utils.Pool;
using Random = UnityEngine.Random;

namespace Manager
{
    public class LootManager : MonoBehaviour
    {
        [SerializeField]
        private Loot blueLootProto;
        [SerializeField]
        private Loot swordLootProto;
        [SerializeField]
        private Loot projectileLootProto;
        [SerializeField]
        private Loot magnetLootProto;
        private ComponentPool<Loot> blueLootPool;
        private ComponentPool<Loot> swordLootPool;
        private ComponentPool<Loot> projectileLootPool;
        private ComponentPool<Loot> magnetLootPool;
        private Dictionary<LootType, ComponentPool<Loot>> lootMapper;
        private Dictionary<LootType, List<Loot>> lootListMapper;
        private int lootQueueIndex = 0;

        public float projectileLootProbability = 10;
        public float magnetLootProbability = 0.1f;

        private void Awake()
        {
            blueLootPool = new ComponentPool<Loot>(blueLootProto);
            swordLootPool = new ComponentPool<Loot>(swordLootProto);
            projectileLootPool = new ComponentPool<Loot>(projectileLootProto);
            magnetLootPool = new ComponentPool<Loot>(magnetLootProto);
            
            lootMapper = new Dictionary<LootType, ComponentPool<Loot>>()
            {
                { LootType.Xp, blueLootPool },
                { LootType.Sword, swordLootPool },
                { LootType.Projectile, projectileLootPool },
                { LootType.Magnet, magnetLootPool }
            };
            lootListMapper = new Dictionary<LootType, List<Loot>>()
            {
                { LootType.Xp, new List<Loot>() },
                { LootType.Projectile, new List<Loot>() },
                { LootType.Magnet, new List<Loot>() }
            };
        }

        private void Update()
        {
            foreach (var lootEntry in lootListMapper)
            {
                for (var i = 0; i < lootEntry.Value.Count; i++)
                {
                    lootEntry.Value[i].Act();
                }
            }
        }

        private Loot CreateLoot(LootType type)
        {
            return lootMapper[type].NewItem();
        }

        private void DestroyLoot(Loot loot)
        {
            
            lootListMapper[loot.lootType].Remove(loot);
            lootMapper[loot.lootType].DestroyItem(loot);
            
        }

        public void DropLoot(Enemy enemy)
        {
            Loot loot;
            var prob = Random.value;
            if (prob <= projectileLootProbability * 0.01f)
            {
                projectileLootProbability = Mathf.Clamp(projectileLootProbability / 1.2f, 0.001f, 10);
                enemy.LootType = LootType.Projectile;
                loot = CreateLoot(enemy.LootType);
                lootListMapper[LootType.Projectile].Add(loot);
            }
            else if (prob <= (projectileLootProbability + magnetLootProbability) * 0.01f)
            {
                enemy.LootType = LootType.Magnet;
                loot = CreateLoot(enemy.LootType);
                lootListMapper[LootType.Magnet].Add(loot);
            }
            else if (lootListMapper[LootType.Xp].Count < 800)
            {
                loot = CreateLoot(enemy.LootType);
                loot.Set(enemy.LootType, 1);
                lootListMapper[LootType.Xp].Add(loot);
            }
            else
            {
                lootQueueIndex++;
                if (lootQueueIndex >= lootListMapper[LootType.Xp].Count)
                {
                    lootQueueIndex = 0;
                }

                loot = lootListMapper[LootType.Xp][lootQueueIndex];
            }

            loot.transform.position = enemy.transform.position;

            if (!loot.OnCollectedRegistered())
            {
                loot.RegisterOnCollected(collectedLoot =>
                {
                    API.PlayerCharacter.LootCollected(collectedLoot);
                    DestroyLoot(collectedLoot);
                });
            }
        }

        public void CollectAll()
        {
            foreach (var loot in lootListMapper[LootType.Xp])
            {
                loot.MoveToPlayer();
            }
            foreach (var loot in lootListMapper[LootType.Projectile])
            {
                loot.MoveToPlayer();
            }
        }
    }
}
