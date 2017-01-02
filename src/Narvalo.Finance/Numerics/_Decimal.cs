// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    internal partial struct _Decimal : IEquatable<_Decimal>, IComparable<_Decimal>, IComparable
    {
        private const short MAX_DECIMAL_PLACES = 28;

        public _Decimal(long value)
        {
            Value = value;
            DecimalPlaces = 0;
        }

        public _Decimal(ulong value)
        {
            Value = value;
            DecimalPlaces = 0;
        }

        public _Decimal(decimal value)
        {
            Value = value;
            DecimalPlaces = MAX_DECIMAL_PLACES;
        }

        public _Decimal(decimal value, short decimalPlaces, MidpointRounding rounding)
        {
            //Require.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES, nameof(decimalPlaces));

            Value = decimalPlaces == MAX_DECIMAL_PLACES
                ? value
                : Round(value, decimalPlaces, rounding);
            DecimalPlaces = decimalPlaces;
        }

        // Only call this one when you know that rounding "value" to "decimalPlaces" will have no effects.
        private _Decimal(decimal value, short decimalPlaces)
        {
            //Demand.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES);

            Value = value;
            DecimalPlaces = decimalPlaces;
        }

        private _Decimal(decimal value, short decimalPlaces, MidpointRounding rounding, bool round)
        {
            //Demand.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES);

            Value = round ? Round(value, decimalPlaces, rounding) : value;
            DecimalPlaces = decimalPlaces;
        }

        public short DecimalPlaces { get; }

        public bool IsIntegral => DecimalPlaces == 0;

        public bool HasFixedScale => DecimalPlaces != MAX_DECIMAL_PLACES;

        public decimal Value { get; }

        private static decimal Round(decimal value, short decimalPlaces, MidpointRounding rounding)
            => decimalPlaces == 0 ? Math.Truncate(value) : Math.Round(value, decimalPlaces, rounding);

        //private _Decimal Create(decimal value, MidpointRounding rounding, bool round)
        //{
        //    return round
        //        ? new _Decimal(value, DecimalPlaces, rounding)
        //        : new _Decimal(value, DecimalPlaces);
        //}

        private bool IsLossy(_Decimal other) => !IsLoseless(other);

        private bool IsLoseless(_Decimal other) => other.DecimalPlaces <= DecimalPlaces;
    }

    // Implements the IEquatable<_Decimal> interface.
    internal partial struct _Decimal
    {
        public static bool operator ==(_Decimal left, _Decimal right) => left.Equals(right);

        public static bool operator !=(_Decimal left, _Decimal right) => !left.Equals(right);

        public bool Equals(_Decimal other) => Value == other.Value && DecimalPlaces == other.DecimalPlaces;

        public override bool Equals(object obj)
        {
            if (!(obj is _Decimal)) { return false; }

            return Equals((_Decimal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + Value.GetHashCode();
                hash = 31 * hash + DecimalPlaces.GetHashCode();
                return hash;
            }
        }
    }

    // Implements the IComparable<NDecimal> and IComparable interfaces.
    internal partial struct _Decimal
    {
        public static bool operator <(_Decimal left, _Decimal right) => left.CompareTo(right) < 0;

        public static bool operator <=(_Decimal left, _Decimal right) => left.CompareTo(right) <= 0;

        public static bool operator >(_Decimal left, _Decimal right) => left.CompareTo(right) > 0;

        public static bool operator >=(_Decimal left, _Decimal right) => left.CompareTo(right) >= 0;

        public int CompareTo(_Decimal other) => Value.CompareTo(other.Value);

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is _Decimal))
            {
                throw new ArgumentException("XXX", nameof(obj));
            }

            return CompareTo((_Decimal)obj);
        }
    }

    // Additions.
    internal partial struct _Decimal
    {
        public _Decimal Add(ulong other)
        {
            if (other == 0UL) { return this; }
            return new _Decimal(Value + other, DecimalPlaces);
        }

        public _Decimal Add(long other)
        {
            if (other == 0L) { return this; }
            return new _Decimal(Value + other, DecimalPlaces);
        }

        public _Decimal Add(decimal other)
        {
            if (other == 0M) { return this; }
            return new _Decimal(Value + other);
        }

        public _Decimal Add(_Decimal other)
        {
            if (Value == 0M) { return other; }
            if (other.Value == 0M) { return this; }
            return new _Decimal(Value + other.Value, Math.Max(DecimalPlaces, other.DecimalPlaces));
        }

        public _Decimal Plus(decimal other, MidpointRounding rounding)
        {
            if (other == 0M) { return this; }
            return new _Decimal(Value + other, DecimalPlaces, rounding);
        }

        public _Decimal Plus(_Decimal other, MidpointRounding rounding)
        {
            if (other.Value == 0M) { return this; }
            return new _Decimal(Value + other.Value, DecimalPlaces, rounding, IsLossy(other));
        }
    }

    // Implements the IFormattable interface.
    //internal partial struct _Decimal
    //{
    //    public string ToString(string format)
    //    {
    //        Warrant.NotNull<string>();
    //        return Value.ToString(format);
    //    }

    //    public string ToString(IFormatProvider formatProvider)
    //    {
    //        Warrant.NotNull<string>();
    //        return Value.ToString(formatProvider);
    //    }

    //    public string ToString(string format, IFormatProvider formatProvider)
    //    {
    //        Warrant.NotNull<string>();
    //        return Value.ToString(format, formatProvider);
    //    }

    //    public override string ToString()
    //    {
    //        Warrant.NotNull<string>();
    //        return Value.ToString();
    //    }
    //}
}

#if CONTRACTS_FULL

namespace Narvalo.Finance.Numerics
{
    using System.Diagnostics.Contracts;

    internal partial struct NDecimal
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_decimalPlaces >= 0 && _decimalPlaces <= MAX_DECIMAL_PLACES);
        }
    }
}

#endif

