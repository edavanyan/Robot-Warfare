using System.Collections;
using DG.Tweening;
using Manager;
using UnityEngine;

namespace Attack
{
    public class MeleeAttacker : Attacker
    {
        [SerializeField] private float attackDuration;
        [SerializeField] private MeleeWeapon weapon;
        [SerializeField] private float weaponRadius;
        private float attackingSpeed;
        [SerializeField]
        private ParticleSystem trail;
        private float tweenValue;
        private bool upgradeScheduled;
        private bool mainWeapon = true;
        
        private void Awake()
        {
            weapon.OnWeaponHit += Attack;
            attackingSpeed = attackDuration / weaponRadius;
        }

        public override void ScheduleUpgrade()
        {
            upgradeScheduled = true;
        }

        protected override IEnumerator FindTargetAndAttack()
        {
            while (CanAttack)
            {

                var scaleX = transform.parent.parent.localScale.x;
                var scaleTime = 0.075f;
                var startAngle = (270 + weaponRadius / 2f) * -scaleX;
                var amountAngle = scaleX * weaponRadius;
                var extraAmount = 0f;
                var extraSpeed = 0f;
                if (weaponRadius > 179)
                {
                    startAngle = 0;
                    amountAngle = scaleX * 179;
                    extraAmount = amountAngle;
                    extraSpeed = attackingSpeed;
                }

                if (InverseSpeed < 0.016f)
                {
                    scaleTime = 0;
                }

                var duration = DOTween.Sequence()
                    .Append(weapon.transform.DORotateQuaternion(
                        Quaternion.Euler(0, 0, startAngle),
                        scaleTime).OnComplete(() => trail.gameObject.SetActive(true)))
                    .Join(weapon.transform.DOScaleY(1, scaleTime))
                    .Append(weapon.transform.DORotateQuaternion(
                            Quaternion.Euler(0, 0, amountAngle),
                            weaponRadius * attackingSpeed)
                        .SetEase(Ease.InSine)
                        .SetRelative(true))
                    .Append(weapon.transform.DORotateQuaternion(
                            Quaternion.Euler(0, 0, extraAmount),
                            weaponRadius * extraSpeed)
                        .SetRelative(true))
                    .AppendCallback(() => trail.gameObject.SetActive(false))
                    .Append(weapon.transform.DORotateQuaternion(
                            Quaternion.Euler(0, 0, 30 * scaleX),
                            scaleTime))
                    .Join(weapon.transform.DOScaleY(0.2f, scaleTime * 2f)).Duration();

                if (upgradeScheduled)
                {
                    upgradeScheduled = false;
                    yield return new WaitForSeconds(0.05f);
                    AddWeapon();
                }

                yield return new WaitForSeconds(InverseSpeed + duration);
            }
        }

        public override void OnLevelUp(int level)
        {
            base.OnLevelUp(level);
            weaponRadius += 5;
        }

        private void AddWeapon()
        {
            if (level <= maxLevel)
            {
                if (mainWeapon)
                {
                    var transform1 = transform;
                    var sword = Instantiate(gameObject, transform1.position, Quaternion.identity, transform1.parent)
                        .GetComponent<MeleeAttacker>();
                    sword.mainWeapon = false;
                    API.PlayerCharacter.attackers.Add(sword);
                }
            }
            else
            {
                attackingSpeed = Mathf.Clamp(attackingSpeed - 0.0001f, 0.001f, 1);
            }
        }
    }
}
