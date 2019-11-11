namespace Basket.Domain.Model
{
    public sealed class ItemId
    {
        public int Value { get; }

        public ItemId(int value)
        {
            Value = value;
        }
    }
}