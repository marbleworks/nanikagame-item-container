using UnityEngine;

namespace NanikaGame.Items
{
    public class Item
    {
        public string Id;

        /// <summary>
        /// Sprite used when displaying this item in the UI.
        /// </summary>
        public Sprite Icon;

        /// <summary>
        /// Price of this item in game currency.
        /// </summary>
        public int Price;

        /// <summary>
        /// Gets or sets a value indicating whether this item is discounted.
        /// </summary>
        public bool IsDiscounted;

        /// <summary>
        /// Price to display when <see cref="IsDiscounted"/> is true.
        /// </summary>
        public int DiscountedPrice;

        /// <summary>
        /// Gets the price that should be used for purchase logic.
        /// Returns <see cref="DiscountedPrice"/> when <see cref="IsDiscounted"/> is true;
        /// otherwise returns <see cref="Price"/>.
        /// </summary>
        public int EffectivePrice => IsDiscounted ? DiscountedPrice : Price;
    }
}