using ItemSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameInput
{
    public class ItemPlacer : MonoBehaviour
    {
        [field: SerializeField] public BuildableItem ActiveBuildable { get; private set; }

        [SerializeField] private float maxBuildDistance = 100f;

        [SerializeField] private ConstructionLayer constructionLayer;

        [SerializeField] private MouseUser mouseUser;

        private void Update()
        {
            if (!IsMouseInRange())
            {
                print("returned");
                return;
            }
            if (mouseUser.IsMouseButtonPressed(MouseButton.LeftMouse) &&
                ActiveBuildable != null &&
                constructionLayer != null)
            {
                constructionLayer.Build(mouseUser.MouseInWorldPos, ActiveBuildable);
            }
        }

        private bool IsMouseInRange()
        {
            var distance = Vector3.Distance(mouseUser.MouseInWorldPos, transform.position);
            return distance <= maxBuildDistance;
        }
    }
}
