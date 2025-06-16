using UnityEngine;
using UnityEngine.EventSystems;

namespace NanikaGame
{
    /// <summary>
    /// UI component that represents an entire <see cref="ItemContainer"/>.
    /// Handles dropping items onto empty space within the container.
    /// </summary>
    public class ItemContainerUI : MonoBehaviour, IDropHandler
    {
        /// <summary>Item container represented by this UI.</summary>
        public ItemContainer Container;

        /// <summary>Prefab used to create slot UIs.</summary>
        public ItemSlotUI SlotPrefab;

        private void Awake()
        {
            if (Container != null)
                Setup(Container);
        }

        /// <summary>
        /// Configures this UI with the given <see cref="ItemContainer"/>.
        /// Existing slot children will be destroyed and recreated.
        /// </summary>
        /// <param name="container">Container to display.</param>
        public void Setup(ItemContainer container)
        {
            Container = container;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if (Container == null || SlotPrefab == null)
                return;

            for (int i = 0; i < Container.Capacity; i++)
            {
                var slot = Instantiate(SlotPrefab, transform);
                slot.Setup(Container, i);
            }
        }

        /// <inheritdoc />
        public void OnDrop(PointerEventData eventData)
        {
            var slot = ItemSlotUI.DraggedSlot;
            if (slot == null || Container == null)
                return;
            // Cancel the move if dropping onto the same container
            if (slot.Container == Container)
                return;

            slot.Container.MoveToFirstEmptySlot(Container, slot.Index);
            slot.Refresh();
        }
    }
}
