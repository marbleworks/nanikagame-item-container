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
        /// Amount of currency the owner currently has.
        /// This value is used when <see cref="GetMoneyFunc"/> is not set.
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// Optional function that returns the current amount of money.
        /// When provided, this value is used for price checks instead of
        /// the <see cref="Money"/> property.
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
        /// with a default capacity of 5 and zero money.
        /// </summary>
        public PriceRestrictedItemContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceRestrictedItemContainer"/> class
        /// with the specified capacity and starting money.
        /// </summary>
        /// <param name="capacity">Container capacity.</param>
        /// <param name="money">Starting amount of money.</param>
        public PriceRestrictedItemContainer(int capacity, int money) : base(capacity)
        {
            Money = money;
        }

        /// <inheritdoc />
        protected override bool CanReceiveItem(Item item, ItemContainer source)
        {
            if (item == null)
                return false;

            var currentMoney = GetMoneyFunc != null ? GetMoneyFunc() : Money;
            return currentMoney >= item.Price;
        }

        /// <inheritdoc />
        protected override void OnItemMovedAway(Item item, ItemContainer destination)
        {
            if (item != null && destination != this)
            {
                UseMoneyAction?.Invoke(item.Price);
                Money -= item.Price;
            }
        }

        /// <inheritdoc />
        protected override void OnItemReceived(Item item, ItemContainer source)
        {
            if (item != null && source != this)
            {
                RefundMoneyAction?.Invoke(item.Price);
                Money += item.Price;
            }
        }
    }
}
