using TMPro;

namespace NanikaGame.Items
{
    /// <summary>
    /// Slot UI for <see cref="SellingItemContainer"/>.
    /// Displays the dragged item's selling price while an item is being dragged.
    /// </summary>
    public class SellingItemSlotUI : ItemSlotUI
    {
        /// <summary>Label used to show the selling price.</summary>
        public TextMeshProUGUI priceLabel;

        private void Update()
        {
            if (priceLabel == null)
                return;

            if (!(Container is SellingItemContainer selling))
                return;

            var dragged = DraggedSlot;
            Item item = null;
            if (dragged != null && selling.WatchedContainers != null &&
                System.Array.IndexOf(selling.WatchedContainers, dragged.Container) >= 0)
            {
                item = dragged.Container.Items[dragged.Index];
            }

            if (item != null)
            {
                int price = selling.GetPriceFunc?.Invoke(item) ?? item.EffectivePrice;
                priceLabel.text = price.ToString();
                priceLabel.enabled = true;
            }
            else
            {
                priceLabel.text = string.Empty;
                priceLabel.enabled = false;
            }
        }
    }
}
