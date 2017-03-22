// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
        public ComparableStruct(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public int CompareTo(ComparableStruct other) => Value.CompareTo(other.Value);

        public static bool operator ==(ComparableStruct left, ComparableStruct right) => left.Equals(right);
        public static bool operator !=(ComparableStruct left, ComparableStruct right) => !left.Equals(right);
        public static bool operator <(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) < 0;
        public static bool operator <=(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) <= 0;
        public static bool operator >(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) > 0;
        public static bool operator >=(ComparableStruct left, ComparableStruct right) => left.CompareTo(right) >= 0;

        public bool Equals(ComparableStruct other) => Value == other.Value;

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            return (obj is ComparableStruct) && Equals((ComparableStruct)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);
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
            return (obj is SimpleStruct) && Equals((SimpleStruct)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);
    }

    public sealed class SimpleObj
    {
        public SimpleObj() { Value = "value"; }

        public SimpleObj(string value) { Value = value; }

        public string Value { get; set; }
    }

    public sealed class ImmutableObj
    {
        public ImmutableObj(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public sealed class EquatableObj : IEquatable<EquatableObj>
    {
        public EquatableObj(string value) { Value = value; }

        public string Value { get; }

        public bool Equals(EquatableObj other)
        {
            if (ReferenceEquals(other, null)) { return false; }
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) { return false; }
            if (ReferenceEquals(obj, this)) { return true; }
            return Equals((EquatableObj)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();
    }

    [Serializable]
    public sealed class SimpleException : Exception
    {
        public SimpleException() : base() { }

        public SimpleException(string message) : base(message) { }

        public SimpleException(string message, Exception innerException)
            : base(message, innerException)
        { }

        private SimpleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
