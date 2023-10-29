using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public class Prop : MonoBehaviour, IPoolable
    {

        public void New()
        {
            gameObject.SetActive(true);
        }

        public void Free()
        {
            gameObject.SetActive(false);
        }
    }
}
