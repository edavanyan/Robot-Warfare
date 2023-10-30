using System;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Utils.Pool;

namespace Grid
{
    public class TerrainChunk : MonoBehaviour, IPoolable
    {
        public ComponentPool<TerrainChunk> Owner { get; set; }
        private new BoxCollider2D collider2D;
        public event Action<TerrainChunk> OnTriggerEnter;

        private void Awake()
        {
            collider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnTriggerEnter?.Invoke(this);
            }
        }

        public void New()
        {
            gameObject.SetActive(true);
            collider2D.enabled = true;
        }

        public void Free()
        {
            OnTriggerEnter = null;
            collider2D.enabled = false;
            gameObject.SetActive(false);
            transform.position = Constants.Nan;
        }
    }
}
