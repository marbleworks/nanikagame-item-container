using System;

namespace NanikaGame
{
    /// <summary>
    /// Item container that only accepts items when the user has enough money
    /// to cover the item's price.
    /// </summary>
    public class PriceRestrictedItemContainer : ItemContainer
    {
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
        /// Initializes a new instance of the <see cref="PriceRestrictedItemContainer"/> class
        /// with a default capacity of 5.
        /// </summary>
        public PriceRestrictedItemContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceRestrictedItemContainer"/> class
        /// with the specified capacity.
        /// </summary>
        /// <param name="capacity">Container capacity.</param>
        public PriceRestrictedItemContainer(int capacity) : base(capacity)
        {
        }

        /// <inheritdoc />
        protected override bool CanReceiveItem(Item item, ItemContainer source)
        {
            if (item == null)
                return false;

            var currentMoney = GetMoneyFunc?.Invoke() ?? 0;
            return currentMoney >= item.Price;
        }

        /// <inheritdoc />
        protected override void OnItemMovedAway(Item item, ItemContainer destination)
        {
            if (item != null && destination != this)
            {
                UseMoneyAction?.Invoke(item.Price);
            }
        }

        /// <inheritdoc />
        protected override void OnItemReceived(Item item, ItemContainer source)
        {
            if (item != null && source != this)
            {
                RefundMoneyAction?.Invoke(item.Price);
            }
        }
    }
}
