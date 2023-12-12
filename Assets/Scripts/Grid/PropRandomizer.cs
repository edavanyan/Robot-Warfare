using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid
{
    public class PropRandomizer : MonoBehaviour
    {
        public List<GameObject> propSpawnPoints;
        public List<GameObject> propPrefabs;

        private List<GameObject> props = new();

        private void OnEnable()
        {
            SpawnProps();
        }

        private void OnDisable()
        {
            foreach (var prop in props)
            {
                Destroy(prop);
            }
            props.Clear();
        }

        void SpawnProps()
        {
            foreach (var spawnPoint in propSpawnPoints)
            {
                var rand = Random.Range(0, propPrefabs.Count);
                props.Add(Instantiate(propPrefabs[rand], spawnPoint.transform.position, Quaternion.identity, transform));
            }
        }
    }
}
