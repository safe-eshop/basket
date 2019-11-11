namespace Basket.Domain.Model
{
    public sealed class ItemId
    {
        public string Value { get; }

        public ItemId(string value)
        {
            Value = value;
        }
    }
}