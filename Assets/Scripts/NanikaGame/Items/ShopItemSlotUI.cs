using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NanikaGame.Items
{
    /// <summary>
    /// Slot UI that displays an item's price in addition to its icon.
    /// </summary>
    public class ShopItemSlotUI : ItemSlotUI
    {
        /// <summary>Toggle used to lock/unlock this slot.</summary>
        public Toggle LockToggle;

        /// <summary>UI text used to show the item's price.</summary>
        public TextMeshProUGUI priceLabel;

        /// <summary>UI text used to show the discounted price.</summary>
        public TextMeshProUGUI discountLabel;

        /// <summary>
        /// Parent object that contains <see cref="discountLabel"/>. This is
        /// toggled on and off when displaying discounted items.
        /// </summary>
        public GameObject discountLabelParent;

        private void Awake()
        {
            if (LockToggle != null)
                LockToggle.onValueChanged.AddListener(OnLockToggleChanged);

            // base Awake simply refreshes the slot
            Refresh();
        }

        private void OnDestroy()
        {
            if (LockToggle != null)
                LockToggle.onValueChanged.RemoveListener(OnLockToggleChanged);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            var item = Container != null ? Container.Items[Index] : null;

            if (LockToggle != null)
            {
                bool hasItem = item != null;
                LockToggle.gameObject.SetActive(hasItem);
                if (hasItem && Container is ShopItemContainer shop)
                    LockToggle.isOn = shop.IsLocked(Index);
            }

            if (priceLabel == null || Container == null)
                return;
            if (item != null)
            {
                priceLabel.text = item.Price.ToString();
                if (item.IsDiscounted)
                {
                    priceLabel.fontStyle = FontStyles.Strikethrough;
                    if (discountLabel != null)
                    {
                        discountLabel.text = item.DiscountedPrice.ToString();
                    }
                    if (discountLabelParent != null)
                        discountLabelParent.SetActive(true);
                    else if (discountLabel != null)
                        discountLabel.enabled = true;
                }
                else
                {
                    priceLabel.fontStyle = FontStyles.Normal;
                    if (discountLabel != null)
                    {
                        discountLabel.text = string.Empty;
                    }
                    if (discountLabelParent != null)
                        discountLabelParent.SetActive(false);
                    else if (discountLabel != null)
                        discountLabel.enabled = false;
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
                }
                if (discountLabelParent != null)
                    discountLabelParent.SetActive(false);
                else if (discountLabel != null)
                    discountLabel.enabled = false;
            }
        }

        private void OnLockToggleChanged(bool isOn)
        {
            if (Container is ShopItemContainer shop)
                shop.SetLocked(Index, isOn);
        }
    }
}
