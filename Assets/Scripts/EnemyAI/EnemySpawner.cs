using Cameras;
using Grid;
using UnityEngine;
using Utils;
using Utils.Pool;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class EnemySpawner : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public new SmoothCamera2D camera;
        public Enemy enemyInstance;
        public float spawnRate;
        public int limit;
        public Map map;

        private ComponentPool<Enemy> enemyPool;

        void Start()
        {
            enemyPool = new ComponentPool<Enemy>(enemyInstance);
            InvokeRepeating(nameof(Spawn), 1f, spawnRate);
        }

        private int count = 0;

        void Spawn()
        {
            if (++count > limit)
            {
                CancelInvoke(nameof(Spawn));
                return;
            }

            var enemyPosition = Constants.Nan;
            while (enemyPosition == Constants.Nan)
            {
                var boundsMax = camera.CameraBoundsMax;
                var boundsMin = camera.CameraBoundsMin;
                var randomAxis = Random.value > 0.5f;
                var isMax = Random.value > 0.5f;
                var spawnPointX = randomAxis ? Random.Range(boundsMin.x, boundsMax.x) :
                    isMax ? boundsMax.x + 1 : boundsMin.x - 1;
                var spawnPointY = randomAxis
                    ? isMax ? boundsMax.y + 1 : boundsMin.y - 1
                    : Random.Range(boundsMin.y, boundsMax.y);
                enemyPosition = map.GetFreeTileAt(spawnPointX, spawnPointY);
            }

            var enemy = enemyPool.NewItem();
            enemy.transform.position = enemyPosition;
            enemy.OnDie += () => enemyPool.DestroyItem(enemy);
        }
    }
}
