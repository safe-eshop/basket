namespace Basket.Domain.Model
{
    public sealed class Item
    {
        public Item(ItemId id, ItemQuantity quantity)
        {
            Id = id;
            Quantity = quantity;
        }

        public ItemId Id { get; }
        public ItemQuantity Quantity { get; }
    }
}