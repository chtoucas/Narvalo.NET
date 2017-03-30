// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

public static partial class My {
    // To be used only when we need a strongly-typed null-string.
    public const string NullString = null;

    public const string WhiteSpaceOnlyString = "     ";

    public enum Enum012 {
        Zero = 0,
        One = 1,
        Two = 2,
        Alias1 = One,
    }

    [Flags]
    public enum EnumBits {
        None = 0,
        One = 1 << 0,
        Two = 1 << 1,
        Four = 1 << 2,
        OneTwo = One | Two,
        OneTwoFour = One | Two | Four
    }

    // An empty struct.
    public struct EmptyVal { }

    public struct ComparableVal : IEquatable<ComparableVal>, IComparable<ComparableVal> {
        public ComparableVal(int value) => Value = value;

        public int Value { get; }

        public int CompareTo(ComparableVal other) => Value.CompareTo(other.Value);

        public static bool operator ==(ComparableVal left, ComparableVal right) => left.Equals(right);
        public static bool operator !=(ComparableVal left, ComparableVal right) => !left.Equals(right);
        public static bool operator <(ComparableVal left, ComparableVal right) => left.CompareTo(right) < 0;
        public static bool operator <=(ComparableVal left, ComparableVal right) => left.CompareTo(right) <= 0;
        public static bool operator >(ComparableVal left, ComparableVal right) => left.CompareTo(right) > 0;
        public static bool operator >=(ComparableVal left, ComparableVal right) => left.CompareTo(right) >= 0;

        public bool Equals(ComparableVal other) => Value == other.Value;

        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            return (obj is ComparableVal) && Equals((ComparableVal)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);
    }

    // A simple value type.
    public struct Val : IEquatable<Val> {
        public Val(int value) => Value = value;

        public int Value { get; }

        public static bool operator ==(Val left, Val right) => left.Equals(right);
        public static bool operator !=(Val left, Val right) => !left.Equals(right);

        public bool Equals(Val other) => Value == other.Value;

        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            return (obj is Val) && Equals((Val)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);
    }

    // A simple reference type.
    public sealed class Obj {
        public Obj() => Value = "value";

        public Obj(string value) => Value = value;

        public string Value { get; set; }

        public override string ToString() => Value;
    }

    // An immutable reference type.
    public sealed class ImmutableObj {
        public ImmutableObj(int value) => Value = value;

        public int Value { get; }
    }

    public sealed class EquatableObj : IEquatable<EquatableObj> {
        public EquatableObj(string value) => Value = value;

        public string Value { get; }

        public bool Equals(EquatableObj other) {
            if (ReferenceEquals(other, null)) { return false; }
            return Value == other.Value;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) { return false; }
            if (ReferenceEquals(obj, this)) { return true; }
            return Equals((EquatableObj)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();
    }

    public sealed class DisposableObj : IDisposable {
        public DisposableObj() { }

        public bool WasDisposed { get; private set; }

        public void Dispose() {
            WasDisposed = true;
        }
    }

    [Serializable]
    public sealed class SimpleException : Exception {
        public SimpleException() : base() { }

        public SimpleException(string message) : base(message) { }

        public SimpleException(string message, Exception innerException)
            : base(message, innerException) { }

        private SimpleException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
