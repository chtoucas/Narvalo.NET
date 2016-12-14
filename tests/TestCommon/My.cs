// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    public static partial class My
    {
        // To be used only when we need a strongly-typed null-string.
        public const string NullString = null;

        public const string WhiteSpaceOnlyString = "     ";

        public enum Enum012
        {
            Zero = 0,
            One = 1,
            Two = 2,
            Alias1 = One,
        }

        [Flags]
        public enum EnumBits
        {
            None = 0,
            One = 1 << 0,
            Two = 1 << 1,
            Four = 1 << 2,
            OneTwo = One | Two,
            OneTwoFour = One | Two | Four
        }

        public struct EmptyStruct { }

        public struct ComparableStruct : IEquatable<ComparableStruct>, IComparable<ComparableStruct>
        {
            private readonly int _value;

            public ComparableStruct(int value)
            {
                _value = value;
            }

            public int CompareTo(ComparableStruct other) => _value.CompareTo(other._value);

            public static bool operator <(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) < 0;

            public static bool operator <=(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) <= 0;

            public static bool operator >(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) > 0;

            public static bool operator >=(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) >= 0;

            public static bool operator ==(ComparableStruct left, ComparableStruct right) => left.Equals(right);

            public static bool operator !=(ComparableStruct left, ComparableStruct right) => !left.Equals(right);

            public bool Equals(ComparableStruct other) => _value == other._value;

            public override bool Equals(object obj)
            {
                if (!(obj is ComparableStruct))
                {
                    return false;
                }

                return Equals((ComparableStruct)obj);
            }

            public override int GetHashCode() => _value.GetHashCode();

            public override string ToString() => _value.ToString(CultureInfo.CurrentCulture);
        }

        public struct SimpleStruct : IEquatable<SimpleStruct>
        {
            public SimpleStruct(int value) { Value = value; }

            public int Value { get; }

            public static bool operator ==(SimpleStruct left, SimpleStruct right) => left.Equals(right);

            public static bool operator !=(SimpleStruct left, SimpleStruct right) => !left.Equals(right);

            public bool Equals(SimpleStruct other) => Value == other.Value;

            public override bool Equals(object obj)
            {
                if (obj == null) { return false; }

                if (!(obj is SimpleStruct)) { return false; }

                return Equals((SimpleStruct)obj);
            }

            public override int GetHashCode() => Value.GetHashCode();
        }

        public sealed class SimpleValue
        {
            public string Value { get; set; }
        }

        public sealed class ImmutableValue
        {
            public ImmutableValue(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        public sealed class EquatableValue : IEquatable<EquatableValue>
        {
            public EquatableValue(string value) { Value = value; }

            public string Value { get; }

            public bool Equals(EquatableValue other)
            {
                if (ReferenceEquals(other, null)) { return false; }

                return Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) { return false; }

                if (ReferenceEquals(obj, this)) { return true; }

                if (obj.GetType() != this.GetType()) { return false; }

                return Equals((EquatableValue)obj);
            }

            public override int GetHashCode() => Value.GetHashCode();
        }

        [Serializable]
        public sealed class SimpleException : Exception
        {
            public SimpleException() : base() { }

            public SimpleException(string message) : base(message) { }

            public SimpleException(string message, Exception innerException)
                : base(message, innerException) { }

            private SimpleException(SerializationInfo info, StreamingContext context)
                : base(info, context) { }
        }
    }
}
