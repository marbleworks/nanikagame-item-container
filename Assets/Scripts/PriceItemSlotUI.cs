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

        /// <summary>UI text used to show the discounted price.</summary>
        public TextMeshProUGUI discountLabel;

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
                if (item.IsDiscounted)
                {
                    priceLabel.fontStyle = FontStyles.Strikethrough;
                    if (discountLabel != null)
                    {
                        discountLabel.text = item.DiscountedPrice.ToString();
                        discountLabel.enabled = true;
                    }
                }
                else
                {
                    priceLabel.fontStyle = FontStyles.Normal;
                    if (discountLabel != null)
                    {
                        discountLabel.text = string.Empty;
                        discountLabel.enabled = false;
                    }
                }

                priceLabel.enabled = true;
            }
            else
            {
                priceLabel.text = string.Empty;
                priceLabel.fontStyle = FontStyles.Normal;
                priceLabel.enabled = false;
                if (discountLabel != null)
                {
                    discountLabel.text = string.Empty;
                    discountLabel.enabled = false;
                }
            }
        }
    }
}
