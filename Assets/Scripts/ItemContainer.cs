using System;
using System.Linq;

namespace NanikaGame
{
    public class ItemContainer
    {
        /// <summary>
        /// コンテナの容量（スロット数）
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// スロットを保持する内部配列
        /// </summary>
        public Item[] Items { get; }

        /// <summary>
        /// アイテムの実際の格納数
        /// </summary>
        public int Count => Items.Count(i => i != null);

        /// <summary>
        /// 変更時に発火するイベント
        /// </summary>
        public event Action OnChangedEvent;

        public bool IsFull => Count == Capacity;
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// デフォルト容量 5 で初期化
        /// </summary>
        public ItemContainer() : this(5) { }

        /// <summary>
        /// 容量を指定して初期化
        /// </summary>
        /// <param name="capacity">スロット数（1 以上）</param>
        public ItemContainer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

            Capacity = capacity;
            Items = new Item[capacity];
        }

        /// <summary>
        /// 配列アイテムで初期化（要素数が容量と同じでないと例外）
        /// </summary>
        /// <param name="items">初期アイテム一覧</param>
        public ItemContainer(Item[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Capacity = items.Length;
            Items = new Item[Capacity];
            Array.Copy(items, Items, Capacity);
        }

        /// <summary>
        /// 全スロットをクリア（null に戻す）
        /// </summary>
        public void Clear()
        {
            Array.Clear(Items, 0, Capacity);
            OnChangedEvent?.Invoke();
        }

        /// <summary>
        /// アイテム配列をまるごと設定
        /// </summary>
        /// <param name="items">設定するアイテム配列（長さが容量と同じでないと false）</param>
        public bool SetItems(Item[] items)
        {
            if (items == null || items.Length != Capacity)
                return false;

            Array.Copy(items, Items, Capacity);
            OnChangedEvent?.Invoke();
            return true;
        }

        /// <summary>
        /// 空きスロットにアイテムを追加
        /// </summary>
        public bool Add(Item item)
        {
            for (var i = 0; i < Capacity; i++)
            {
                if (Items[i] != null) continue;
                Items[i] = item;
                OnChangedEvent?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 最初に見つかったアイテムを削除
        /// </summary>
        public bool Remove(Item item)
        {
            for (var i = 0; i < Capacity; i++)
            {
                if (Items[i] != item) continue;
                Items[i] = null;
                OnChangedEvent?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// インデックス指定で削除
        /// </summary>
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= Capacity || Items[index] == null)
                return false;

            Items[index] = null;
            OnChangedEvent?.Invoke();
            return true;
        }

        /// <summary>
        /// （オプション）削除後に詰めるヘルパー
        /// </summary>
        private void ShiftLeft(int start)
        {
            for (var i = start; i < Capacity - 1; i++)
                Items[i] = Items[i + 1];
            Items[Capacity - 1] = null;
            OnChangedEvent?.Invoke();
        }
    }
}
