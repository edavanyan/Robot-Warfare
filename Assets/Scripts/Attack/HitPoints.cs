using System;
using UnityEngine;

namespace Attack
{
    public class HitPoints
    {
        public int MaxHitPoints { get; private set; }
        public int CurrentHitPoints { get; private set; }
        public event Action OnDie;
        public event Action<int> OnDamaged;

        public HitPoints(int maxHitPoints)
        {
            MaxHitPoints = maxHitPoints;
            Reset();
        }

        public void IncreaseMaxHp(int amount)
        {
            var ratio = (float)CurrentHitPoints / MaxHitPoints;
            MaxHitPoints += amount;
            CurrentHitPoints = (int)Mathf.Ceil(MaxHitPoints * ratio);
        }

        public bool Hit(int damage)
        {
            CurrentHitPoints -= damage;
            OnDamaged?.Invoke(damage);
            if (CurrentHitPoints <= 0)
            {
                CurrentHitPoints = 0;
                OnDie?.Invoke();
                OnDie = null;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            CurrentHitPoints = MaxHitPoints;
        }
    }
}
