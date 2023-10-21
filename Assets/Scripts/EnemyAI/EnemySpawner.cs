using Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class EnemySpawner : MonoBehaviour
    {
        public Camera camera;
        public GameObject enemyInstance;
        public int limit;
        public Map map;

        void Start()
        {
            InvokeRepeating(nameof(Spawn), 1f, 0.1f);
        }

        private int count = 0;
        void Spawn()
        {
            if (++count > limit)
            {
                CancelInvoke(nameof(Spawn));
                return;
            }
            var index = Random.Range(0, map.freeTiles.Count);
            var cell = map.freeTiles[index];
            var enemyPosition = cell.Value;

            Instantiate(enemyInstance, enemyPosition, Quaternion.identity);
        }
    }
}