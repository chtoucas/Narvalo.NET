// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal partial struct _Decimal : IEquatable<_Decimal>, IComparable<_Decimal>, IComparable //, IFormattable
    {
        public const MidpointRounding DefaultRounding = MidpointRounding.ToEven;

        private const int MAX_DECIMAL_PLACES = 28;

        public _Decimal(decimal value)
        {
            Value = value;
            DecimalPlaces = MAX_DECIMAL_PLACES;
            Normalized = false;
        }

        public _Decimal(decimal value, int decimalPlaces, MidpointRounding rounding)
        {
            Require.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES, nameof(decimalPlaces));

            Value = Math.Round(value, decimalPlaces, rounding);
            DecimalPlaces = decimalPlaces;
            Normalized = true;
        }

        // Only call this one when "decimalPlaces" is known to be lower than or equal to
        // the one stored inside "value".
        private _Decimal(decimal value, int decimalPlaces)
        {
            Demand.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES);

            Value = value;
            DecimalPlaces = decimalPlaces;
            Normalized = true;
        }

        public int DecimalPlaces { get; }

        public bool Normalized { get; }

        public decimal Value { get; }

        private _Decimal Map(decimal value, MidpointRounding rounding, int decimalPlaces)
            => decimalPlaces <= DecimalPlaces
            ? new _Decimal(value, DecimalPlaces)
            : new _Decimal(value, DecimalPlaces, rounding);

        //private bool IsLossy(NDecimal other) => !IsLoseless(other);

        //private bool IsLoseless(NDecimal other) => other.DecimalPlaces <= DecimalPlaces;

        //private void ThrowIfLossy(NDecimal other)
        //{
        //    if (other.DecimalPlaces > DecimalPlaces)
        //    {
        //        throw new InvalidOperationException();
        //    }
        //}
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

    // Overrides the op_Addition operator.
    internal partial struct _Decimal
    {
        public static _Decimal operator +(_Decimal left, _Decimal right) => left.Add(right);

        public _Decimal Add(_Decimal other) => Add(other, DefaultRounding);

        public _Decimal Add(_Decimal other, MidpointRounding rounding)
        {
            if (other.Value == 0M) { return this; }

            return Map(Value + other.Value, rounding, other.DecimalPlaces);
        }

        public _Decimal Add(IEnumerable<_Decimal> others) => Add(others, DefaultRounding);

        public _Decimal Add(IEnumerable<_Decimal> others, MidpointRounding rounding)
        {
            Require.NotNull(others, nameof(others));

            var sum = Value + others.Sum(_ => _.Value);

            return new _Decimal(sum, DecimalPlaces, rounding);
        }
    }
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

