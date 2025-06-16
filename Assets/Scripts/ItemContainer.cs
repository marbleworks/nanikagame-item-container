using System;
using System.Linq;

namespace NanikaGame
{
    public class ItemContainer
    {
        public int Capacity { get; }

        public Item[] Items { get; }

        public int Count => Items.Count(i => i != null);

        public event Action Changed;

        public bool IsFull => Count == Capacity;
        public bool IsEmpty => Count == 0;

        public ItemContainer() : this(5)
        {
        }

        public ItemContainer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

            Capacity = capacity;
            Items = new Item[capacity];
        }

        public ItemContainer(Item[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Capacity = items.Length;
            Items = new Item[Capacity];
            Array.Copy(items, Items, Capacity);
        }

        public void Clear()
        {
            Array.Clear(Items, 0, Capacity);
            Changed?.Invoke();
        }

        public bool SetItems(Item[] items)
        {
            if (items == null || items.Length != Capacity)
                return false;

            Array.Copy(items, Items, Capacity);
            Changed?.Invoke();
            return true;
        }

        public bool Set(int index, Item item)
        {
            if (!IsIndexValid(index))
                return false;

            Items[index] = item;
            Changed?.Invoke();
            return true;
        }

        public bool Add(Item item)
        {
            var index = Array.IndexOf(Items, null);
            return index != -1 && Set(index, item);
        }

        public bool Remove(Item item)
        {
            var index = Array.IndexOf(Items, item);
            return index != 0 && RemoveAt(index);
        }

        public bool RemoveAt(int index)
        {
            if (!IsIndexValid(index) || Items[index] == null)
                return false;

            Items[index] = null;
            Changed?.Invoke();
            return true;
        }

        private bool IsIndexValid(int index)
        {
            return index >= 0 && index < Capacity;
        }
    }
}
