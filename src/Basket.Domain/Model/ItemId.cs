namespace Basket.Domain.Model
{
    public struct ItemId
    {
        public string Value { get; }

        public ItemId(string value)
        {
            Value = value;
        }

        public bool Equals(ItemId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public void Deconstruct(out string value)
        {
            value = Value;
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }
}