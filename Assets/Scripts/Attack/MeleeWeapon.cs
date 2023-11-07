using System;
using UnityEngine;

namespace Attack
{
    public class MeleeWeapon : MonoBehaviour
    {
        public Action<Transform> OnWeaponHit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                OnWeaponHit?.Invoke(other.transform);
            }
        }
    }
}
