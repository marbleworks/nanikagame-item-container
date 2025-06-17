using System;
using System.Collections.Generic;

namespace NanikaGame
{
    /// <summary>
    /// Item container that only accepts items when the user has enough money
    /// to cover the item's price. Swapping with items from other containers
    /// is disallowed by default.
    /// </summary>
    public class ShopItemContainer : ItemContainer
    {
        /// <summary>
        /// Gets the lock state for each slot. The length matches <see cref="Capacity"/>.
        /// </summary>
        public bool[] LockedSlots { get; }

        /// <summary>
        /// Optional function that returns the current amount of money.
        /// When provided, this value is used for price checks.
        /// </summary>
        public Func<int> GetMoneyFunc { get; set; }

        /// <summary>
        /// Callback invoked when money should be spent. The item's price is
        /// provided as the argument.
        /// </summary>
        public Action<int> UseMoneyAction { get; set; }

        /// <summary>
        /// Callback invoked when money should be refunded. The item's price is
        /// provided as the argument.
        /// </summary>
        public Action<int> RefundMoneyAction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItemContainer"/> class
        /// with a default capacity of 5.
        /// </summary>
        public ShopItemContainer()
        {
            AllowExternalSwap = false;
            LockedSlots = new bool[Capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItemContainer"/> class
        /// with the specified capacity.
        /// </summary>
        /// <param name="capacity">Container capacity.</param>
        public ShopItemContainer(int capacity) : base(capacity)
        {
            AllowExternalSwap = false;
            LockedSlots = new bool[capacity];
        }

        /// <inheritdoc />
        protected override bool CanSendItem(Item item, ItemContainer destination)
        {
            if (item == null || destination == this)
                return true;

            var currentMoney = GetMoneyFunc?.Invoke() ?? 0;
            return currentMoney >= item.EffectivePrice;
        }

        /// <inheritdoc />
        protected override bool CanReceiveItem(Item item, ItemContainer source)
        {
            if (item == null)
                return false;

            var currentMoney = GetMoneyFunc?.Invoke() ?? 0;
            return currentMoney >= item.EffectivePrice;
        }

        /// <inheritdoc />
        protected override void OnItemMovedAway(Item item, ItemContainer destination)
        {
            if (item != null && destination != this)
            {
                UseMoneyAction?.Invoke(item.EffectivePrice);
            }
        }

        /// <inheritdoc />
        protected override void OnItemReceived(Item item, ItemContainer source)
        {
            if (item != null && source != this)
            {
                RefundMoneyAction?.Invoke(item.EffectivePrice);
            }
        }

        /// <summary>
        /// Sets the lock state of the specified slot.
        /// </summary>
        /// <param name="index">Slot index.</param>
        /// <param name="locked">True to lock the slot; false to unlock.</param>
        public void SetLocked(int index, bool locked)
        {
            if (!IsIndexValid(index))
                return;

            if (LockedSlots[index] == locked)
                return;

            LockedSlots[index] = locked;
            OnChanged();
        }

        /// <summary>
        /// Gets whether the specified slot is locked.
        /// </summary>
        /// <param name="index">Slot index.</param>
        public bool IsLocked(int index)
        {
            return IsIndexValid(index) && LockedSlots[index];
        }

        /// <summary>
        /// Returns the indices of all locked slots.
        /// </summary>
        public int[] GetLockedIndices()
        {
            var list = new System.Collections.Generic.List<int>();
            for (int i = 0; i < Capacity; i++)
            {
                if (LockedSlots[i])
                    list.Add(i);
            }

            return list.ToArray();
        }
    }
}
