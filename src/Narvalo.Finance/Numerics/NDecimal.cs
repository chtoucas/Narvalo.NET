// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial struct NDecimal : IEquatable<NDecimal>, IComparable<NDecimal>, IComparable, IFormattable
    {
        public const MidpointRounding DefaultRounding = MidpointRounding.ToEven;

        private const int MAX_DECIMAL_PLACES = 28;

        private readonly decimal _value;
        private readonly int _decimalPlaces;

        //public NDecimal(byte value) { _value = value; _decimalPlaces = 0; }

        //[CLSCompliant(false)]
        //public NDecimal(sbyte value) { _value = value; _decimalPlaces = 0; }

        //[CLSCompliant(false)]
        //public NDecimal(ushort value) { _value = value; _decimalPlaces = 0; }

        //public NDecimal(short value) { _value = value; _decimalPlaces = 0; }

        //[CLSCompliant(false)]
        //public NDecimal(uint value) { _value = value; _decimalPlaces = 0; }

        //public NDecimal(int value) { _value = value; _decimalPlaces = 0; }

        //[CLSCompliant(false)]
        //public NDecimal(ulong value) { _value = value; _decimalPlaces = 0; }

        //public NDecimal(long value) { _value = value; _decimalPlaces = 0; }

        public NDecimal(decimal value)
        {
            _value = value;
            _decimalPlaces = MAX_DECIMAL_PLACES;
        }

        public NDecimal(decimal value, int decimalPlaces, MidpointRounding rounding)
        {
            Require.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES, nameof(decimalPlaces));

            _value = Math.Round(value, decimalPlaces, rounding);
            _decimalPlaces = decimalPlaces;
        }

        // Only call this one when "decimalPlaces" is known to be lower than or equal to
        // the one stored inside "value".
        private NDecimal(decimal value, int decimalPlaces)
        {
            Demand.Range(decimalPlaces >= 0 && decimalPlaces <= MAX_DECIMAL_PLACES);

            _value = value;
            _decimalPlaces = decimalPlaces;
        }

        public int DecimalPlaces => _decimalPlaces;

        public decimal Value => _value;

        private NDecimal Map(decimal value, MidpointRounding rounding, int decimalPlaces)
            => decimalPlaces <= DecimalPlaces
            ? new NDecimal(value, DecimalPlaces)
            : new NDecimal(value, DecimalPlaces, rounding);

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
    public partial struct NDecimal
    {
        public string ToString(string format)
        {
            Warrant.NotNull<string>();
            return Value.ToString(format);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();
            return Value.ToString(formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();
            return Value.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();
            return Value.ToString();
        }
    }

    // Implements the IEquatable<_Decimal> interface.
    public partial struct NDecimal
    {
        public static bool operator ==(NDecimal left, NDecimal right) => left.Equals(right);

        public static bool operator !=(NDecimal left, NDecimal right) => !left.Equals(right);

        public bool Equals(NDecimal other) => Value == other.Value && DecimalPlaces == other.DecimalPlaces;

        public override bool Equals(object obj)
        {
            if (!(obj is NDecimal)) { return false; }

            return Equals((NDecimal)obj);
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

    // Conversions.
    public partial struct NDecimal
    {
        public static byte ToByte(NDecimal value) => Decimal.ToByte(value.Value);

        [CLSCompliant(false)]
        public static sbyte ToSByte(NDecimal value) => Decimal.ToSByte(value.Value);

        [CLSCompliant(false)]
        public static ushort ToUInt16(NDecimal value) => Decimal.ToUInt16(value.Value);

        public static short ToInt16(NDecimal value) => Decimal.ToInt16(value.Value);

        [CLSCompliant(false)]
        public static uint ToUInt32(NDecimal value) => Decimal.ToUInt32(value.Value);

        public static int ToInt32(NDecimal value) => Decimal.ToInt32(value.Value);

        [CLSCompliant(false)]
        public static ulong ToUInt64(NDecimal value) => Decimal.ToUInt64(value.Value);

        public static long ToInt64(NDecimal value) => Decimal.ToInt64(value.Value);

        public static decimal ToDecimal(NDecimal value) => value.Value;

        #region Conversions to NDecimal.

        public static implicit operator NDecimal(byte value) => new NDecimal(value);

        [CLSCompliant(false)]
        public static implicit operator NDecimal(sbyte value) => new NDecimal(value);

        [CLSCompliant(false)]
        public static implicit operator NDecimal(ushort value) => new NDecimal(value);

        public static implicit operator NDecimal(short value) => new NDecimal(value);

        [CLSCompliant(false)]
        public static implicit operator NDecimal(uint value) => new NDecimal(value);

        public static implicit operator NDecimal(int value) => new NDecimal(value);

        [CLSCompliant(false)]
        public static implicit operator NDecimal(ulong value) => new NDecimal(value);

        public static implicit operator NDecimal(long value) => new NDecimal(value);

        public static implicit operator NDecimal(decimal value) => new NDecimal(value);

        #endregion

        #region Conversions from NDecimal.

        public static explicit operator byte(NDecimal value) => ToByte(value);

        [CLSCompliant(false)]
        public static explicit operator sbyte(NDecimal value) => ToSByte(value);

        [CLSCompliant(false)]
        public static explicit operator ushort(NDecimal value) => ToUInt16(value);

        public static explicit operator short(NDecimal value) => ToInt16(value);

        [CLSCompliant(false)]
        public static explicit operator uint(NDecimal value) => ToUInt32(value);

        public static explicit operator int(NDecimal value) => ToInt32(value);

        [CLSCompliant(false)]
        public static explicit operator ulong(NDecimal value) => ToUInt64(value);

        public static explicit operator long(NDecimal value) => ToInt64(value);

        // NB: This one is implicit (no loss of precision).
        public static implicit operator decimal(NDecimal value) => ToDecimal(value);

        #endregion
    }

    // Implements the IComparable<NDecimal> and IComparable interfaces.
    public partial struct NDecimal
    {
        public static bool operator <(NDecimal left, NDecimal right) => left.CompareTo(right) < 0;

        public static bool operator <=(NDecimal left, NDecimal right) => left.CompareTo(right) <= 0;

        public static bool operator >(NDecimal left, NDecimal right) => left.CompareTo(right) > 0;

        public static bool operator >=(NDecimal left, NDecimal right) => left.CompareTo(right) >= 0;

        public int CompareTo(NDecimal other) => Value.CompareTo(other.Value);

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is NDecimal))
            {
                throw new ArgumentException("XXX", nameof(obj));
            }

            return CompareTo((NDecimal)obj);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct NDecimal
    {
        public static NDecimal operator +(NDecimal left, NDecimal right) => left.Add(right);

        public NDecimal Add(NDecimal other) => Add(other, DefaultRounding);

        public NDecimal Add(NDecimal other, MidpointRounding rounding)
        {
            if (other.Value == 0M) { return this; }

            return Map(Value + other.Value, rounding, other.DecimalPlaces);
        }

        public NDecimal Add(IEnumerable<NDecimal> others) => Add(others, DefaultRounding);

        public NDecimal Add(IEnumerable<NDecimal> others, MidpointRounding rounding)
        {
            Require.NotNull(others, nameof(others));

            var sum = Value + others.Sum(_ => _.Value);

            return new NDecimal(sum, DecimalPlaces, rounding);
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance.Numerics
{
    using System.Diagnostics.Contracts;

    public partial struct NDecimal
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_decimalPlaces >= 0 && _decimalPlaces <= MAX_DECIMAL_PLACES);
        }
    }
}

#endif

