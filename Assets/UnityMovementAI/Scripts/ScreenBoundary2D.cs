using UnityEngine;

namespace UnityMovementAI.Scripts
{
    public class ScreenBoundary2D : MonoBehaviour
    {
        Vector3 bottomLeft;
        Vector3 topRight;
        Vector3 widthHeight;

        void Start()
        {
            float distAway = Mathf.Abs(UnityEngine.Camera.main.transform.position.z);

            bottomLeft = UnityEngine.Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distAway));
            topRight = UnityEngine.Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distAway));
            widthHeight = topRight - bottomLeft;

            transform.localScale = new Vector3(widthHeight.x, widthHeight.y, transform.localScale.z);
        }

        void OnTriggerStay2D(Collider2D other)
        {
            KeepInBounds(other);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            KeepInBounds(other);
        }

        void KeepInBounds(Collider2D other)
        {
            Transform t = other.transform;

            if (t.position.x < bottomLeft.x)
            {
                t.position = new Vector3(t.position.x + widthHeight.x, t.position.y, t.position.z);
            }

            if (t.position.x > topRight.x)
            {
                t.position = new Vector3(t.position.x - widthHeight.x, t.position.y, t.position.z);
            }

            if (t.position.y < bottomLeft.y)
            {
                t.position = new Vector3(t.position.x, t.position.y + widthHeight.y, t.position.z);
            }

            if (t.position.y > topRight.y)
            {
                t.position = new Vector3(t.position.x, t.position.y - widthHeight.y, t.position.z);
            }
        }
    }
}