using UnityEngine;

namespace PlayerController
{
    public class SpawnPoints : MonoBehaviour
    {
        void Awake()
        {
            transform.parent = Camera.main.transform;
        }
    }
}
