using System;
using EnemyAI;
using PlayerController;
using UnityEngine;
using Utils.Pool;

namespace Loots
{
    public class LootManager : MonoBehaviour
    {
        [SerializeField]
        private Loot lootProto;
        private ComponentPool<Loot> lootPool;
        [SerializeField] private Animator lootBlue;
        [SerializeField] private Animator lootGreen;
        [SerializeField] private Animator lootCandy;
        [SerializeField] private Animator lootCoin;

        private void Awake()
        {
            lootPool = new ComponentPool<Loot>(lootProto);
        }

        public void DropLoot(Enemy enemy)
        {
            var loot = lootPool.NewItem();
            loot.transform.position = enemy.transform.position;
            loot.Set(LootType.Xp, 1, lootBlue.runtimeAnimatorController);
            loot.onCollected += () =>
            {
                PlayerCharacterProvider.PlayerCharacter.LootCollected(loot);
                lootPool.DestroyItem(loot);
            };
        }
    }
}
