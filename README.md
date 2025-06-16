## NanikaGame Item Container

This repository contains scripts for an item container system in Unity.

### Item

- **Price**: Integer value representing the in-game cost of the item.

### ItemContainer

- **AllowInternalMove**: When set to `false`, items cannot be moved between slots in the same container. This is enabled by default.
- **AllowExternalMove**: When set to `false`, this container will not accept
  items moved from other containers.

### PriceRestrictedItemContainer

- **Money**: Current amount of currency available.
- **GetMoneyFunc**: Optional function returning the current amount of money. When set, this is used for price checks instead of `Money`.
- Items can only be moved *into* this container if the available money (from `GetMoneyFunc` or `Money`) is at least the item's `Price`.
- Items can only be moved *out of* this container when `GetMoneyFunc` reports funds equal to or exceeding the item's `Price`.
- **UseMoneyAction**: Optional callback invoked with an item's price when it leaves the container. `Money` decreases by that amount.
- **RefundMoneyAction**: Optional callback invoked with an item's price when it is returned.
  `Money` increases by that amount.
