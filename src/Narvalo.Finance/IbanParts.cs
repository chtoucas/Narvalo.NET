// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    public partial struct IbanParts : IEquatable<IbanParts>, IFormattable
    {
        internal const string DefaultFormat = "G";
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

        private const char WHITESPACE_CHAR = ' ';

        public const string HumanHeader = "IBAN ";

        private readonly string _bban;
        private readonly string _checkDigits;
        private readonly string _countryCode;
        private readonly string _value;

        private IbanParts(string countryCode, string checkDigits, string bban)
            : this(countryCode, checkDigits, bban, countryCode + checkDigits + bban)
        {
            Expect.NotNull(countryCode);
            Expect.NotNull(checkDigits);
            Expect.NotNull(bban);
        }

        private IbanParts(string countryCode, string checkDigits, string bban, string value)
        {
            Demand.NotNull(countryCode);
            Demand.NotNull(checkDigits);
            Demand.NotNull(bban);
            Demand.NotNull(value);

            _bban = bban;
            _checkDigits = checkDigits;
            _countryCode = countryCode;
            _value = value;
        }

        public string Bban
        {
            get { Warrant.NotNull<string>(); return _bban; }
        }

        public string CheckDigits
        {
            get { Warrant.NotNull<string>(); return _checkDigits; }
        }

        public string CountryCode
        {
            get { Warrant.NotNull<string>(); return _countryCode; }
        }

        internal string LiteralValue => _value;

        public static IbanParts Build(string countryCode, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(bban, nameof(bban));
            Enforce.True(CountryPart.Validate(countryCode), nameof(countryCode));
            Enforce.True(BbanPart.Validate(bban), nameof(bban));

            var checkDigits = IbanCheckDigits.Compute(countryCode, bban);

            return new IbanParts(countryCode, checkDigits, bban);
        }

        public static IbanParts Create(string countryCode, string checkDigits, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));
            Enforce.True(CountryPart.Validate(countryCode), nameof(countryCode));
            Enforce.True(CheckDigitsPart.Validate(checkDigits), nameof(checkDigits));
            Enforce.True(BbanPart.Validate(bban), nameof(bban));

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

        public static Outcome<IbanParts> TryParse(string value)
        {
            if (!CheckLength(value)) { return Outcome<IbanParts>.Failure(Strings.Parse_InvalidIbanValue); }

            string countryCode = CountryPart.FromIban(value);
            if (countryCode == null) { return Outcome<IbanParts>.Failure(Strings.Parse_InvalidCountryCode); }

            string checkDigits = CheckDigitsPart.FromIban(value);
            if (checkDigits == null) { return Outcome<IbanParts>.Failure(Strings.Parse_InvalidCheckDigits); }

            string bban = BbanPart.FromIban(value);
            if (bban == null) { return Outcome<IbanParts>.Failure(Strings.Parse_InvalidBban); }

            return Outcome.Success(new IbanParts(countryCode, checkDigits, bban, value));
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
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();
            return ToString(DefaultFormat, null);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();
            return ToString(format, null);
        }

        /// <inheritdoc cref="IFormattable.ToString(string, IFormatProvider)" />
        // NB: We ignore any user supplied "formatProvider".
        public string ToString(string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

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
            Warrant.NotNull<string>();

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

        public override bool Equals(object obj)
        {
            if (!(obj is IbanParts)) { return false; }

            return Equals((IbanParts)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public partial struct IbanParts
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_bban != null);
            Contract.Invariant(_checkDigits != null);
            Contract.Invariant(_countryCode != null);
            Contract.Invariant(_value != null);
        }
    }
}

#endif
