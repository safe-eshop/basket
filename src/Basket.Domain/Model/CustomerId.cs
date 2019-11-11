using System;

namespace Basket.Domain.Model
{
    public sealed class CustomerId
    {
        public CustomerId(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static CustomerId Crate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return new CustomerId(value);
        }

        private bool Equals(CustomerId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is CustomerId other && Equals(other);
        }

        public void Deconstruct(out string value)
        {
            value = Value;
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}