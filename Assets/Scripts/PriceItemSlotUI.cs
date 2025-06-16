using UnityEngine.UI;

namespace NanikaGame
{
    /// <summary>
    /// Slot UI that displays an item's price in addition to its icon.
    /// </summary>
    public class PriceItemSlotUI : ItemSlotUI
    {
        /// <summary>UI text used to show the item's price.</summary>
        public Text PriceLabel;

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            if (PriceLabel == null || Container == null)
                return;

            var item = Container.Items[Index];
            if (item != null)
            {
                PriceLabel.text = item.Price.ToString();
                PriceLabel.enabled = true;
            }
            else
            {
                PriceLabel.text = string.Empty;
                PriceLabel.enabled = false;
            }
        }
    }
}
