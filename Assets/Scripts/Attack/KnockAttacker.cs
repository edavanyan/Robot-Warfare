using UnityEngine;
using Utils.Pool;

namespace Attack
{
    public class KnockAttacker : Attacker
    {
        [SerializeField] private Animator vfx;
        [SerializeField] private EnemyProjectile enemyProjProto;
        private static readonly int Punch = Animator.StringToHash("Punch");

        private ComponentPool<EnemyProjectile> enemyProjPool;

        private void Awake()
        {
            enemyProjPool = new ComponentPool<EnemyProjectile>(enemyProjProto);
        }

        // ReSharper disable once ParameterHidesMember
        protected override void OnHit(Transform target, Projectile projectile)
        {
            vfx.gameObject.SetActive(true);
            vfx.SetTrigger(Punch);
            Invoke(nameof(HideEffect), 0.417f);
            base.OnHit(target, projectile);
        }

        private void HideEffect()
        {
            vfx.gameObject.SetActive(false);
        }

        public override void OnLevelUp(int level)
        {
            damage += 6;
            AttackSpeed += 1;
        }
    }
}
