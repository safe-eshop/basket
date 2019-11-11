namespace Basket.Domain.Model
{
    public sealed class CustomerId
    {
        public CustomerId(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}