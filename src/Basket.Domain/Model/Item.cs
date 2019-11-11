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

        private bool Equals(Item other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Item other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public Item NewQuantity(ItemQuantity quantity)
        {
            return null;
        }
        
        public static Item Create(string productId, int quantity)
        {
            return new Item(new ItemId(productId), new ItemQuantity(quantity));
        }

        public Item IncreaseQuantity(ItemQuantity itemQuantity)
        {
            return new Item(Id, Quantity.Increase(itemQuantity));
        }
    }
}