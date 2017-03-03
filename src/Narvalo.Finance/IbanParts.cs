// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    public partial struct IbanParts : IEquatable<IbanParts>, IFormattable
    {
        internal const string DefaultFormat = "G";
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

        private const char WHITESPACE_CHAR = ' ';

        public const string HumanHeader = "IBAN ";

        private readonly string _value;

        private IbanParts(string countryCode, string checkDigits, string bban)
            : this(countryCode, checkDigits, bban, countryCode + checkDigits + bban)
        {
        }

        private IbanParts(string countryCode, string checkDigits, string bban, string value)
        {
            Demand.NotNull(countryCode);
            Demand.NotNull(checkDigits);
            Demand.NotNull(bban);
            Demand.NotNull(value);

            Bban = bban;
            CheckDigits = checkDigits;
            CountryCode = countryCode;
            _value = value;
        }

        public string Bban { get; }

        public string CheckDigits { get; }

        public string CountryCode { get; }

        internal string LiteralValue => _value;

        public static IbanParts Build(string countryCode, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(bban, nameof(bban));
            Require.True(CountryPart.Validate(countryCode), nameof(countryCode));
            Require.True(BbanPart.Validate(bban), nameof(bban));

            var checkDigits = IbanCheckDigits.Compute(countryCode, bban);

            return new IbanParts(countryCode, checkDigits, bban);
        }

        public static IbanParts Create(string countryCode, string checkDigits, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));
            Require.True(CountryPart.Validate(countryCode), nameof(countryCode));
            Require.True(CheckDigitsPart.Validate(checkDigits), nameof(checkDigits));
            Require.True(BbanPart.Validate(bban), nameof(bban));

            return new IbanParts(countryCode, checkDigits, bban);
        }

        public static IbanParts? Parse(string value)
        {
            if (!CheckLength(value)) { return null; }

            string countryCode = CountryPart.FromIban(value);
            if (countryCode == null) { return null; }

            string checkDigits = CheckDigitsPart.FromIban(value);
            if (checkDigits == null) { return null; }

            string bban = BbanPart.FromIban(value);
            if (bban == null) { return null; }

            return new IbanParts(countryCode, checkDigits, bban, value);
        }

        public static Result<IbanParts> TryParse(string value)
        {
            if (!CheckLength(value)) { return Result<IbanParts>.FromError(Strings.Parse_InvalidIbanValue); }

            string countryCode = CountryPart.FromIban(value);
            if (countryCode == null) { return Result<IbanParts>.FromError(Strings.Parse_InvalidCountryCode); }

            string checkDigits = CheckDigitsPart.FromIban(value);
            if (checkDigits == null) { return Result<IbanParts>.FromError(Strings.Parse_InvalidCheckDigits); }

            string bban = BbanPart.FromIban(value);
            if (bban == null) { return Result<IbanParts>.FromError(Strings.Parse_InvalidBban); }

            return Result.Of(new IbanParts(countryCode, checkDigits, bban, value));
        }

        internal static bool CheckLength(string value)
            => value != null && value.Length >= MinLength && value.Length <= MaxLength;

        private static class BbanPart
        {
            public const int StartIndex = CountryPart.Length + CheckDigitsPart.Length;
            public const int MinLength = IbanParts.MinLength - StartIndex;
            public const int MaxLength = IbanParts.MaxLength - StartIndex;

            public static string FromIban(string value)
            {
                Demand.Range(value.Length >= StartIndex);
                var retval = value.Substring(StartIndex);
                return CheckContent(retval) ? retval : null;
            }

            public static bool Validate(string value)
            {
                Demand.NotNull(value);
                return value.Length >= MinLength && value.Length <= MaxLength && CheckContent(value);
            }

            private static bool CheckContent(string value) => Ascii.IsDigitOrUpperLetter(value);
        }

        private static class CheckDigitsPart
        {
            public const int StartIndex = CountryPart.Length;
            public const int Length = 2;

            public static string FromIban(string value)
            {
                Demand.Range(value.Length >= StartIndex + Length);
                var retval = value.Substring(StartIndex, Length);
                return CheckContent(retval) ? retval : null;
            }

            public static bool Validate(string value)
            {
                Demand.NotNull(value);
                return value.Length == Length && CheckContent(value);
            }

            private static bool CheckContent(string value) => Ascii.IsDigit(value);
        }

        private static class CountryPart
        {
            public const int StartIndex = 0;
            public const int Length = 2;

            public static string FromIban(string value)
            {
                Demand.Range(value.Length >= StartIndex + Length);
                var retval = value.Substring(StartIndex, Length);
                return CheckContent(retval) ? retval : null;
            }

            public static bool Validate(string value)
            {
                Demand.NotNull(value);
                return value.Length == Length && CheckContent(value);
            }

            private static bool CheckContent(string value) => Ascii.IsUpperLetter(value);
        }
    }

    // Implements the IFormattable interface.
    public partial struct IbanParts
    {
        public override string ToString() => ToString(DefaultFormat, null);

        public string ToString(string format) => ToString(format, null);

        // NB: We ignore any user supplied "formatProvider".
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null || format.Length == 0) { format = DefaultFormat; }
            if (format.Length != 1) { throw new FormatException("XXX"); }

            // Take the first char and uppercase it (ASCII only).
            switch (format[0] & 0xDF)
            {
                case 'C':
                    // Compact.
                    return _value;
                case 'H':
                    // Human: same result as "G" but prefixed with "IBAN ".
                    // This format is NOT suitable for electronic transmission.
                    return HumanHeader + FormatGeneral(_value);
                case 'G':
                    // General (default): insert a whitespace char every 4 chars.
                    // This format is NOT suitable for electronic transmission.
                    return FormatGeneral(_value);
                default:
                    throw new FormatException(Format.Current(Strings.Iban_InvalidFormatSpecification));
            }
        }

        private static string FormatGeneral(string input)
        {
            Demand.NotNull(input);

            int len = input.Length;

            int rem;
            int div = Number.DivRem(len, 4, out rem);

            int outlen = len + div - (rem == 0 ? 1 : 0);
            var output = new char[outlen];

            int k = 1;
            for (var i = 0; i < len; i++, k++)
            {
                if (k % 5 == 0)
                {
                    output[k - 1] = WHITESPACE_CHAR;
                    output[k] = input[i];
                    k++;
                }
                else
                {
                    output[k - 1] = input[i];
                }
            }

            return new String(output);
        }
    }

    // Implements the IEquatable<IbanParts> interface.
    public partial struct IbanParts
    {
        public static bool operator ==(IbanParts left, IbanParts right) => left.Equals(right);

        public static bool operator !=(IbanParts left, IbanParts right) => !left.Equals(right);

        public bool Equals(IbanParts other) => _value == other._value;

        public override bool Equals(object obj) => (obj is IbanParts) && Equals((IbanParts)obj);

        public override int GetHashCode() => _value.GetHashCode();
    }
}
