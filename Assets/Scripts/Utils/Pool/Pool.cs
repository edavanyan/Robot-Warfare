using System.Collections.Generic;
using Attack;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils.Pool
{
    public abstract class Pool<T> where T : IPoolable
    {
        private readonly List<T> freeItemList = new ();
        private readonly T prototype;

        protected Pool(T prototype)
        {
            this.prototype = prototype;
        }

        public T NewItem()
        {
            if (freeItemList.Count > 0)
            {
                var item = freeItemList[0];
                freeItemList.RemoveAt(0);

                item.New();
                return item;
            }
            
            var newItem = CreateItem(prototype);
            newItem.New();
            return newItem;
        }

        protected abstract T CreateItem(T prototype);

        public void DestroyItem(T item)
        {
            if (freeItemList.Contains(item))
            {
                return;
            }
            freeItemList.Add(item);
            item.Free();
        }
    }
}
