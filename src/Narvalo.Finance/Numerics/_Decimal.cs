// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal partial struct _Decimal : IEquatable<_Decimal>
    {
        private const int SIGN_MASK = unchecked((int)0x80000000);
        //private const byte DECIMAL_NEG = 0x80;
        //private const byte DECIMAL_ADD = 0x00;
        private const int SCALE_MASK = 0x00FF0000;
        private const int SCALE_SHIFT = 16;
        // The maximum power of 10 that a 32 bit integer can store.
        //private const int MAX_INT_SCALE = 9;

        private readonly decimal _value;
        private readonly int _decimalPlaces;

        private _Decimal(decimal value, int decimalPlaces)
        {
            _value = value;
            _decimalPlaces = decimalPlaces;
        }

        public int DecimalPlaces => _decimalPlaces;

        public decimal Value => _value;

        public _Decimal(byte value) : this(value, 0) { }

        //[CLSCompliant(false)]
        public _Decimal(sbyte value) : this(value, 0) { }

        //[CLSCompliant(false)]
        public _Decimal(ushort value) : this(value, 0) { }

        public _Decimal(short value) : this(value, 0) { }

        //[CLSCompliant(false)]
        public _Decimal(uint value) : this(value, 0) { }

        public _Decimal(int value) : this(value, 0) { }

        //[CLSCompliant(false)]
        public _Decimal(ulong value) : this(value, 0) { }

        public _Decimal(long value) : this(value, 0) { }

        //public static _Decimal Of(decimal value) => new _Decimal(value, GetScale(value));

        public static _Decimal OfScale(decimal value, int decimalPlaces)
            => OfScale(value, decimalPlaces, MidpointRounding.ToEven);

        public static _Decimal OfScale(decimal value, int decimalPlaces, MidpointRounding rounding)
        {
            var val = Math.Round(value, decimalPlaces, rounding);

            return new _Decimal(val, decimalPlaces);
        }

        // https://msdn.microsoft.com/en-us/library/system.decimal.getbits.aspx
        // GetBits() returns an array
        // - The first, second and third elements represent the low, middle, and high 32 bits
        //   of the 96-bit integer number.
        // - The fourth element contains the scale factor and sign:
        //   * bits 0 to 15, the lower word, are unused and must be zero.
        //   * bits 16 to 23 must contain an exponent between 0 and 28 inclusive,
        //     which indicates the power of 10 to divide the integer number.
        //   * bits 24 to 30 are unused and must be zero.
        //   * bit 31 contains the sign: 0 mean positive, and 1 means negative.

        private static int GetScale(decimal value)
        {
            int flags = Decimal.GetBits(value)[3];
            // Bits 16 to 23 contains an exponent between 0 and 28, which indicates the power
            // of 10 to divide the integer number.
            //return (flags >> 16) & 0xFF;
            return (flags & SCALE_MASK) >> SCALE_SHIFT;
        }

        private static bool GetSign(decimal value)
        {
            int flags = Decimal.GetBits(value)[3];
            // The last bit contains the sign: 0 means positive, and 1 means negative.
            return (flags & SIGN_MASK) != 0;
        }

        private _Decimal MapValue(decimal value) => new _Decimal(value, DecimalPlaces);

        private decimal Round(decimal value, MidpointRounding rounding)
            => Math.Round(value, DecimalPlaces, rounding);

        private bool IsLossy(_Decimal other) => other.DecimalPlaces > DecimalPlaces;

        private bool IsLoseless(_Decimal other) => !IsLossy(other);

        private void ThrowIfLossy(_Decimal other)
        {
            if (other.DecimalPlaces > DecimalPlaces)
            {
                throw new InvalidOperationException();
            }
        }
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

    // Overrides the op_Addition operator.
    internal partial struct _Decimal
    {
        public static _Decimal operator +(_Decimal left, _Decimal right) => left.Add(right);

        public static _Decimal operator +(_Decimal left, decimal right) => left.Add(right);

        public static _Decimal operator +(decimal left, _Decimal right) => right.Add(left);

        public _Decimal Add(_Decimal other)
        {
            if (other.Value == 0M) { return this; }
            ThrowIfLossy(other);

            return MapValue(Value + other.Value);
        }

        public _Decimal Add(decimal other)
        {
            if (other == 0M) { return this; }

            return MapValue(Value + other);
        }

        public _Decimal? TryAdd(_Decimal other)
        {
            if (other.Value == 0M) { return this; }
            if (IsLossy(other)) { return null; }

            return MapValue(Value + other.Value);
        }

        public _Decimal AddUnchecked(_Decimal other) => AddUnchecked(other, MidpointRounding.ToEven);

        public _Decimal AddUnchecked(_Decimal other, MidpointRounding rounding)
        {
            if (other.Value == 0M) { return this; }

            decimal sum = Value + other.Value;

            return MapValue(IsLoseless(other) ? sum : Round(sum, rounding));
        }

        public _Decimal Add(IEnumerable<_Decimal> others)
        {
            Require.NotNull(others, nameof(others));

            decimal sum = 0M;
            foreach (var item in others)
            {
                ThrowIfLossy(item);

                sum += item.Value;
            }

            return MapValue(sum);
        }

        public _Decimal? TryAdd(IEnumerable<_Decimal> others)
        {
            if (others == null) { return null; }

            decimal sum = 0M;
            foreach (var item in others)
            {
                if (IsLossy(item)) { return null; }

                sum += item.Value;
            }

            return MapValue(sum);
        }

        public _Decimal AddUnchecked(IEnumerable<_Decimal> others) => AddUnchecked(others, MidpointRounding.ToEven);

        public _Decimal AddUnchecked(IEnumerable<_Decimal> others, MidpointRounding rounding)
        {
            Require.NotNull(others, nameof(others));

            decimal sum = Value + others.Sum(_ => _.Value);

            return MapValue(Round(sum, rounding));
        }
    }
}
