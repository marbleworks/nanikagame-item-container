using TMPro;
using UnityEngine;

namespace NanikaGame
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

            var dragged = DraggedSlot;
            var item = dragged != null ? dragged.Container.Items[dragged.Index] : null;
            if (item != null)
            {
                priceLabel.text = item.EffectivePrice.ToString();
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
