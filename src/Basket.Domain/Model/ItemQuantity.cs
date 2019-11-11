namespace Basket.Domain.Model
{
    public sealed class ItemQuantity
    {
        public ItemQuantity(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}