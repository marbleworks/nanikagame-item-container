using System;
using System.Linq;

namespace NanikaGame.Items
{
    /// <summary>
    /// Represents a container that can hold a fixed number of items.
    /// Raises an event when its contents change.
    /// </summary>
    public class ItemContainer
    {
        /// <summary>
        /// Gets the maximum number of items the container can hold.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Gets the array of items currently stored in the container.
        /// A null entry represents an empty slot.
        /// </summary>
        public Item[] Items { get; }


        /// <summary>
        /// Gets the number of non-null items currently in the container.
        /// </summary>
        public int Count => Items.Count(i => i != null);

        /// <summary>
        /// Event that is invoked whenever the contents of the container change.
        /// </summary>
        public event Action Changed;

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        protected void OnChanged()
        {
            Changed?.Invoke();
        }

        /// <summary>
        /// Gets a value indicating whether the container is full.
        /// </summary>
        public bool IsFull => Count == Capacity;

        /// <summary>
        /// Gets a value indicating whether the container is empty.
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Gets or sets a value indicating whether items can be moved
        /// between slots within this container.
        /// </summary>
        public bool AllowInternalMove { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this container accepts
        /// moves from other containers.
        /// </summary>
        public bool AllowExternalMove { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether items in this container can
        /// be swapped with items from another container. This is enabled by
        /// default.
        /// </summary>
        public bool AllowExternalSwap { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of containers that are not allowed to move
        /// items into this container.
        /// </summary>
        public ItemContainer[] DisallowedSources { get; set; } =
            Array.Empty<ItemContainer>();

        /// <summary>
        /// Determines whether this container is willing to accept the given
        /// <paramref name="item"/> from the specified <paramref name="source"/>.
        /// Derived containers can override this to impose custom restrictions
        /// on item transfers.
        /// </summary>
        /// <param name="item">Item that is being moved.</param>
        /// <param name="source">Container from which the item originates.</param>
        /// <returns>True if the item can be added; otherwise false.</returns>
        protected virtual bool CanReceiveItem(Item item, ItemContainer source)
        {
            return true;
        }

        /// <summary>
        /// Determines whether this container is willing to give the specified
        /// <paramref name="item"/> to the <paramref name="destination"/>.
        /// Derived containers can override this to impose custom restrictions
        /// before an item leaves the container.
        /// </summary>
        /// <param name="item">Item that will be moved out.</param>
        /// <param name="destination">Container that wants to receive the item.</param>
        /// <returns>True if the item can be removed; otherwise false.</returns>
        protected virtual bool CanSendItem(Item item, ItemContainer destination)
        {
            return true;
        }

        /// <summary>
        /// Called after an item is successfully moved from this container to
        /// another container.
        /// </summary>
        /// <param name="item">Item that was moved out.</param>
        /// <param name="index">Slot index the item occupied.</param>
        /// <param name="destination">Container that received the item.</param>
        protected virtual void OnItemMovedAway(Item item, int index, ItemContainer destination)
        {
        }

        /// <summary>
        /// Called after an item has been received from another container.
        /// </summary>
        /// <param name="item">Item that was added.</param>
        /// <param name="index">Slot index where the item was placed.</param>
        /// <param name="source">Container from which the item originated.</param>
        protected virtual void OnItemReceived(Item item, int index, ItemContainer source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainer"/> class
        /// with a default capacity of 5.
        /// </summary>
        public ItemContainer() : this(5)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainer"/> class
        /// with the specified capacity.
        /// </summary>
        /// <param name="capacity">The maximum number of items the container can hold. Must be greater than zero.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="capacity"/> is less than or equal to zero.
        /// </exception>
        public ItemContainer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

            Capacity = capacity;
            Items = new Item[capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainer"/> class
        /// using an existing array of items.
        /// </summary>
        /// <param name="items">Array of items to populate the container. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="items"/> is null.
        /// </exception>
        public ItemContainer(Item[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Capacity = items.Length;
            Items = new Item[Capacity];
            Array.Copy(items, Items, Capacity);
        }

        /// <summary>
        /// Removes all items from the container and triggers the <see cref="Changed"/> event.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Items, 0, Capacity);
            OnChanged();
        }

        /// <summary>
        /// Replaces the container's items with the specified array, if the arrays match in length.
        /// </summary>
        /// <param name="items">New items to set. Must have the same length as the container's capacity.</param>
        /// <returns>True if the items were successfully set; otherwise false.</returns>
        public bool SetItems(Item[] items)
        {
            if (items == null || items.Length != Capacity)
                return false;

            Array.Copy(items, Items, Capacity);
            OnChanged();
            return true;
        }


        /// <summary>
        /// Sets the item at the given index and triggers the <see cref="Changed"/> event.
        /// </summary>
        /// <param name="index">The zero-based slot in which to place the item.</param>
        /// <param name="item">The item to place in the container.</param>
        /// <returns>True if the item was successfully set; otherwise false.</returns>
        public bool Set(int index, Item item)
        {
            if (!IsIndexValid(index))
                return false;

            Items[index] = item;
            OnChanged();
            return true;
        }

        /// <summary>
        /// Adds the specified item to the first available empty slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>True if the item was added; false if the container is full.</returns>
        public bool Add(Item item)
        {
            var index = Array.IndexOf(Items, null);
            return index != -1 && Set(index, item);
        }

        /// <summary>
        /// Removes the specified item from the container.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was found and removed; otherwise false.</returns>
        public bool Remove(Item item)
        {
            var index = Array.IndexOf(Items, item);
            return index >= 0 && RemoveAt(index);
        }

        /// <summary>
        /// Removes the item at the specified index and triggers the <see cref="Changed"/> event.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <returns>True if the item was removed; otherwise false.</returns>
        public bool RemoveAt(int index)
        {
            if (!IsIndexValid(index) || Items[index] == null)
                return false;

            Items[index] = null;
            OnChanged();
            return true;
        }

        /// <summary>
        /// Checks whether the given index is within the valid range of slots.
        /// </summary>
        /// <param name="index">The index to validate.</param>
        /// <returns>True if index is between 0 and Capacity - 1; otherwise false.</returns>
        public bool IsIndexValid(int index)
        {
            return index >= 0 && index < Capacity;
        }

        /// <summary>
        /// Moves the item from <paramref name="fromIndex"/> to <paramref name="toIndex"/>
        /// inside this container or another container. If the destination slot
        /// already has an item, the items are swapped.
        /// </summary>
        /// <param name="destination">Destination container.</param>
        /// <param name="fromIndex">Index of the item to move in the source container.</param>
        /// <param name="toIndex">Index of the slot in the destination container.</param>
        /// <returns>True if the move succeeded; otherwise false.</returns>
        public bool MoveItem(ItemContainer destination, int fromIndex, int toIndex)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (!IsIndexValid(fromIndex) || !destination.IsIndexValid(toIndex))
                return false;

            if (destination == this && !AllowInternalMove)
                return false;
            if (destination != this && !destination.AllowExternalMove)
                return false;
            if (destination != this && destination.DisallowedSources != null &&
                Array.IndexOf(destination.DisallowedSources, this) >= 0)
                return false;

            var item = Items[fromIndex];
            if (item == null)
                return false;

            var destItem = destination.Items[toIndex];

            if (destItem != null && destination != this && (!AllowExternalSwap || !destination.AllowExternalSwap))
                return false;

            if (!CanSendItem(item, destination))
                return false;

            if (!destination.CanReceiveItem(item, this))
                return false;

            Items[fromIndex] = destItem;
            destination.Items[toIndex] = item;

            if (destination != this)
            {
                OnItemMovedAway(item, fromIndex, destination);
                destination.OnItemReceived(item, toIndex, this);

                if (destItem != null)
                {
                    destination.OnItemMovedAway(destItem, toIndex, this);
                    OnItemReceived(destItem, fromIndex, destination);
                }
            }

            OnChanged();
            if (destination != this)
                destination.OnChanged();

            return true;
        }

        /// <summary>
        /// Moves the item from <paramref name="fromIndex"/> to the first empty
        /// slot in <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">Destination container.</param>
        /// <param name="fromIndex">Index of the item to move.</param>
        /// <returns>True if the move succeeded; otherwise false.</returns>
        public bool MoveToFirstEmptySlot(ItemContainer destination, int fromIndex)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (!IsIndexValid(fromIndex))
                return false;

            if (destination == this && !AllowInternalMove)
                return false;
            if (destination != this && !destination.AllowExternalMove)
                return false;
            if (destination != this && destination.DisallowedSources != null &&
                Array.IndexOf(destination.DisallowedSources, this) >= 0)
                return false;

            var item = Items[fromIndex];
            if (item == null)
                return false;

            if (!CanSendItem(item, destination))
                return false;

            if (!destination.CanReceiveItem(item, this))
                return false;

            var emptyIndex = Array.IndexOf(destination.Items, null);
            if (emptyIndex == -1)
                return false;

            Items[fromIndex] = null;
            destination.Items[emptyIndex] = item;

            if (destination != this)
            {
                OnItemMovedAway(item, fromIndex, destination);
                destination.OnItemReceived(item, emptyIndex, this);
            }

            OnChanged();
            if (destination != this)
                destination.OnChanged();

            return true;
        }
    }
}
