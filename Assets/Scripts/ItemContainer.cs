using System;
using System.Linq;

namespace NanikaGame
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
        /// Gets a value indicating whether the container is full.
        /// </summary>
        public bool IsFull => Count == Capacity;

        /// <summary>
        /// Gets a value indicating whether the container is empty.
        /// </summary>
        public bool IsEmpty => Count == 0;

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
            Changed?.Invoke();
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
            Changed?.Invoke();
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
            Changed?.Invoke();
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
            Changed?.Invoke();
            return true;
        }

        /// <summary>
        /// Checks whether the given index is within the valid range of slots.
        /// </summary>
        /// <param name="index">The index to validate.</param>
        /// <returns>True if index is between 0 and Capacity - 1; otherwise false.</returns>
        private bool IsIndexValid(int index)
        {
            return index >= 0 && index < Capacity;
        }
    }
}
