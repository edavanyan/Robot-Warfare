using System;
using UnityEngine;

namespace Attack
{
    public class Melee : Projectile
    {
        // ReSharper disable once ParameterHidesMember
        public override void Init(Transform target, Action<Transform, Projectile> hitCallback)
        {
            hitCallback(target, this);
        }

        public override void New()
        {
            gameObject.SetActive(true);
        }

        public override void Free()
        {
            base.Free();
            gameObject.SetActive(false);
        }

        public override void Act()
        {
            
        }
    }
}
