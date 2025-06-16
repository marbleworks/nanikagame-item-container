using UnityEngine;

namespace NanikaGame
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
    }
}