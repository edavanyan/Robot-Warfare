using Unity.VisualScripting;
using UnityEngine;

namespace Utils.Pool
{
    public class ComponentPool<T> : Pool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Transform defaultParent;
        public ComponentPool(T prototype) : base(prototype)
        {
        }

        public ComponentPool(T prototype, Transform transform, bool forceInstantiate = true) : base(prototype)
        {
            defaultParent = transform;
            if (forceInstantiate)
            {
                var proto = CreateItem(prototype);
                DestroyItem(proto);
            }
        }

        protected sealed override T CreateItem(T prototype)
        {
            return GameObject.Instantiate(prototype, defaultParent);
        }

    }
}