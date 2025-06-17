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

- **Money**: Tracks money earned by selling items.
- When an item enters the container, its `EffectivePrice` is added to `Money` and the item is removed.
- Items cannot be moved out once placed in the container.

### SellingItemSlotUI

- Displays the dragged item\x27s selling price while an item is being dragged.
