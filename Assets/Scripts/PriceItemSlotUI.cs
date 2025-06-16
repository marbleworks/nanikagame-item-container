using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NanikaGame
{
    /// <summary>
    /// Slot UI that displays an item's price in addition to its icon.
    /// </summary>
    public class PriceItemSlotUI : ItemSlotUI
    {
        /// <summary>UI text used to show the item's price.</summary>
        public TextMeshProUGUI priceLabel;

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            if (priceLabel == null || Container == null)
                return;

            var item = Container.Items[Index];
            if (item != null)
            {
                priceLabel.text = item.Price.ToString();
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
