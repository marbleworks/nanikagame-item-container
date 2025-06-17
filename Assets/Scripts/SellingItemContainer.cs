using System;

namespace NanikaGame
{
    /// <summary>
    /// Item container that automatically converts received items into money.
    /// Items disappear when added to this container.
    /// </summary>
    public class SellingItemContainer : ItemContainer
    {
        /// <summary>
        /// Containers monitored for drag events.
        /// </summary>
        public ItemContainer[] WatchedContainers { get; set; } = Array.Empty<ItemContainer>();

        /// <summary>
        /// Callback that returns the selling price for a given item.
        /// If not provided, <see cref="Item.EffectivePrice"/> is used.
        /// </summary>
        public Func<Item, int> GetPriceFunc { get; set; }

        /// <summary>
        /// Optional callback invoked with the item's price when sold.
        /// </summary>
        public Action<int> AddMoneyAction { get; set; }

        /// <inheritdoc />
        protected override bool CanSendItem(Item item, ItemContainer destination)
        {
            // Items cannot be moved out once sold.
            return false;
        }

        /// <inheritdoc />
        protected override void OnItemReceived(Item item, int index, ItemContainer source)
        {
            if (item == null)
                return;

            int price = GetPriceFunc?.Invoke(item) ?? item.EffectivePrice;
            AddMoneyAction?.Invoke(price);

            // Remove the item from this container after selling it.
            Items[index] = null;
        }
    }
}
