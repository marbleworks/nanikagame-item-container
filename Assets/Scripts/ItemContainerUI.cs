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

        /// <inheritdoc />
        public void OnDrop(PointerEventData eventData)
        {
            var slot = ItemSlotUI.DraggedSlot;
            if (slot == null || Container == null)
                return;

            slot.Container.MoveToFirstEmptySlot(Container, slot.Index);
            slot.Refresh();
        }
    }
}
