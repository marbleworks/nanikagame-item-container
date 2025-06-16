using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NanikaGame
{
    /// <summary>
    /// UI component representing a single slot in an <see cref="ItemContainer"/>.
    /// Supports drag and drop between slots and containers.
    /// </summary>
    public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        /// <summary>Associated item container.</summary>
        public ItemContainer Container;

        /// <summary>Slot index within the container.</summary>
        public int Index;

        /// <summary>Image used to display the item.</summary>
        public Image Icon;

        /// <summary>Currently dragged slot.</summary>
        public static ItemSlotUI DraggedSlot { get; private set; }

        private void Awake()
        {
            if (Container != null)
            {
                Container.Changed += Refresh;
            }
            Refresh();
        }

        /// <summary>Refreshes the icon visibility.</summary>
        public void Refresh()
        {
            if (Icon == null || Container == null)
                return;

            Icon.enabled = Container.Items[Index] != null;
        }

        /// <inheritdoc />
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Container == null)
                return;
            if (Container.Items[Index] == null)
                return;

            DraggedSlot = this;
        }

        /// <inheritdoc />
        public void OnEndDrag(PointerEventData eventData)
        {
            DraggedSlot = null;
        }

        /// <inheritdoc />
        public void OnDrop(PointerEventData eventData)
        {
            if (DraggedSlot == null || Container == null)
                return;

            // Attempt to move or swap the item
            if (!DraggedSlot.Container.MoveItem(Container, DraggedSlot.Index, Index))
            {
                // If move failed and dropping onto the same container, try to move to first empty slot
                DraggedSlot.Container.MoveToFirstEmptySlot(Container, DraggedSlot.Index);
            }

            DraggedSlot.Refresh();
            Refresh();
        }
    }
}
