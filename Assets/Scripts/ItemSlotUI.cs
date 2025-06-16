using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NanikaGame
{
    /// <summary>
    /// UI component representing a single slot in an <see cref="ItemContainer"/>.
    /// Supports drag and drop between slots and containers.
    /// </summary>
    public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        /// <summary>Associated item container.</summary>
        public ItemContainer Container;

        /// <summary>Slot index within the container.</summary>
        public int Index;

        /// <summary>Image used to display the item.</summary>
        public Image Icon;

        /// <summary>Currently dragged slot.</summary>
        public static ItemSlotUI DraggedSlot { get; private set; }

        /// <summary>Icon displayed while dragging.</summary>
        private static Image dragIcon;

        private void Awake()
        {
            Refresh();
        }

        private void OnEnable()
        {
            if (Container != null)
                Container.Changed += Refresh;
        }

        private void OnDisable()
        {
            if (Container != null)
                Container.Changed -= Refresh;
        }

        /// <summary>
        /// Configures this slot with the given container and index.
        /// Can be called after the object is instantiated.
        /// </summary>
        /// <param name="container">Container to display.</param>
        /// <param name="index">Slot index.</param>
        public void Setup(ItemContainer container, int index)
        {
            if (Container != null)
                Container.Changed -= Refresh;

            Container = container;
            Index = index;

            if (isActiveAndEnabled && Container != null)
                Container.Changed += Refresh;

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
            if (Container == null || Icon == null)
                return;
            if (Container.Items[Index] == null)
                return;

            DraggedSlot = this;

            var canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                return;

            dragIcon = new GameObject("DragIcon").AddComponent<Image>();
            dragIcon.sprite = Icon.sprite;
            dragIcon.transform.SetParent(canvas.transform, false);
            dragIcon.transform.SetAsLastSibling();
            dragIcon.raycastTarget = false;
            dragIcon.rectTransform.sizeDelta = Icon.rectTransform.sizeDelta;
            dragIcon.rectTransform.position = eventData.position;

            // Hide the icon in the original slot while dragging
            Icon.enabled = false;
        }

        /// <inheritdoc />
        public void OnDrag(PointerEventData eventData)
        {
            if (dragIcon != null)
                dragIcon.rectTransform.position = eventData.position;
        }

        /// <inheritdoc />
        public void OnEndDrag(PointerEventData eventData)
        {
            DraggedSlot = null;
            if (dragIcon != null)
            {
                Destroy(dragIcon.gameObject);
                dragIcon = null;
            }

            // Restore icon visibility based on container contents
            Refresh();
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
