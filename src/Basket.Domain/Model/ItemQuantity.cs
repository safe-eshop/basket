namespace Basket.Domain.Model
{
    public struct ItemQuantity
    {
        public ItemQuantity(int value)
        {
            Value = value;
        }

        public int Value { get; }


        public ItemQuantity Increase(ItemQuantity val)
        {
            return new ItemQuantity(Value + val.Value);
        }

        public static ItemQuantity Create(int val)
        {
            return new ItemQuantity(val);
        }

        public bool Equals(ItemQuantity other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemQuantity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public void Deconstruct(out int value)
        {
            value = Value;
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }
}