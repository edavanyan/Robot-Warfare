using System.Collections.Generic;
using DG.Tweening;
using EnemyAI;
using Loots;
using PlayerController;
using UnityEngine;
using Utils.Pool;

namespace Manager
{
    public class LootManager : MonoBehaviour
    {
        [SerializeField]
        private Loot blueLootProto;
        [SerializeField]
        private Loot swordLootProto;
        private ComponentPool<Loot> blueLootPool;
        private ComponentPool<Loot> swordLootPool;
        private Dictionary<LootType, ComponentPool<Loot>> lootMapper;

        private void Awake()
        {
            blueLootPool = new ComponentPool<Loot>(blueLootProto);
            swordLootPool = new ComponentPool<Loot>(swordLootProto);
            lootMapper = new Dictionary<LootType, ComponentPool<Loot>>()
            {
                { LootType.Xp, blueLootPool },
                { LootType.Sword, swordLootPool }
            };
        }

        private Loot CreateLoot(LootType type)
        {
            return lootMapper[type].NewItem();
        }

        public void DropLoot(Enemy enemy)
        {
            var loot = CreateLoot(enemy.LootType);
            loot.Set(enemy.LootType, 1);
            
            loot.transform.position = enemy.transform.position;
            if (loot.lootType == LootType.Sword)
            {
                loot.circleCollider2D.enabled = false;
                DOTween.Sequence()
                    .Append(loot.transform.DOMove(Vector2.right + Vector2.up / 2, 0.15f).SetRelative(true))
                    .Append(loot.transform.DOMove(Vector2.right - Vector2.up / 2, 0.15f).SetRelative(true))
                    .OnComplete(() => loot.circleCollider2D.enabled = true);
            }

            loot.onCollected += () =>
            {
                API.PlayerCharacter.LootCollected(loot);
                lootMapper[loot.lootType].DestroyItem(loot);
            };
        }
    }
}
