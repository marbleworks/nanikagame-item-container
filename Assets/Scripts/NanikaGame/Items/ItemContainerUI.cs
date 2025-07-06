using UnityEngine;
using UnityEngine.EventSystems;

namespace NanikaGame.Items
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

            if (slot.Container == Container)
            {
                // When dropping onto empty space in the same container, move the
                // item to the earliest available slot only if that slot is
                // before the item's current position.
                int emptyIndex = System.Array.IndexOf(Container.Items, null);
                if (emptyIndex >= 0 && emptyIndex < slot.Index)
                {
                    slot.Container.MoveItem(Container, slot.Index, emptyIndex);
                    slot.Refresh();
                }
                return;
            }

            slot.Container.MoveToFirstEmptySlot(Container, slot.Index);
            slot.Refresh();
        }
    }
}
