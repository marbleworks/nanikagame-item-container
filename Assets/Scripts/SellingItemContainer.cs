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
        /// Current amount of money earned from sold items.
        /// </summary>
        public int Money { get; private set; }

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

            Money += item.EffectivePrice;
            AddMoneyAction?.Invoke(item.EffectivePrice);

            // Remove the item from this container after selling it.
            Items[index] = null;
        }
    }
}
