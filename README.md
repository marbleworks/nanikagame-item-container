## NanikaGame Item Container

This repository contains scripts for an item container system in Unity.

### Item

- **Price**: Integer value representing the in-game cost of the item.
- **IsDiscounted**: Indicates whether the item is currently on sale.
- **DiscountedPrice**: Price shown when the item is discounted.
- **EffectivePrice**: Returns `DiscountedPrice` when the item is on sale;
  otherwise returns `Price`.

### ItemContainer

- **AllowInternalMove**: When set to `false`, items cannot be moved between slots in the same container. This is enabled by default.
- **AllowExternalMove**: When set to `false`, this container will not accept
  items moved from other containers.
- **AllowExternalSwap**: When set to `false`, this container cannot swap items
  with other containers.
- **DisallowedSources**: Array of containers that are not allowed to move items
  into this container.

### ShopItemContainer

- **Money**: Current amount of currency available.
- **GetMoneyFunc**: Optional function returning the current amount of money. When set, this is used for price checks instead of `Money`.
- Items can only be moved *into* this container if the available money (from `GetMoneyFunc` or `Money`) is at least the item's `EffectivePrice`.
- Items can only be moved *out of* this container when `GetMoneyFunc` reports funds equal to or exceeding the item's `EffectivePrice`.
- **UseMoneyAction**: Optional callback invoked with an item's price when it leaves the container. `Money` decreases by that amount.
- **RefundMoneyAction**: Optional callback invoked with an item's price when it is returned.
  `Money` increases by that amount.
- Swapping items with other containers is disabled by default.

### SellingItemContainer

- **WatchedContainers**: Array of containers monitored for drag events.
- **GetPriceFunc**: Callback used to determine the selling price of each item.
- When an item enters the container, it is passed to `SellItemAction` and then removed.
- **SellItemAction**: Optional callback invoked when an item is sold. The item itself is supplied so the sale price can be determined using `GetPriceFunc` or `EffectivePrice`.
- Items cannot be moved out once placed in the container.

### SellingItemSlotUI

- Displays the selling price of an item being dragged from a container listed in `WatchedContainers`.
- Uses `GetPriceFunc` from the associated <code>SellingItemContainer</code> to obtain the price.
