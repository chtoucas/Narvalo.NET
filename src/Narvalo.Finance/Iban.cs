// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.AsciiHelpers;
    using static Narvalo.Finance.IbanFormat;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Iban : IEquatable<Iban>, IFormattable
    {
        private const string DefaultFormat = "G";

        private readonly string _bban;
        private readonly string _checkDigits;
        private readonly string _countryCode;
        private readonly string _value;

        private Iban(string countryCode, string checkDigits, string bban, string value, bool validated)
        {
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckCheckDigits(checkDigits));
            Demand.True(CheckBban(bban));
            Demand.True(CheckValue(value));

            _countryCode = countryCode;
            _checkDigits = checkDigits;
            _bban = bban;
            _value = value;
            Verified = validated;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get { Sentinel.Warrant.LengthRange(BbanMinLength, BbanMaxLength); return _bban; }
        }

        /// <summary>
        /// Gets the check digits.
        /// </summary>
        public string CheckDigits
        {
            get { Sentinel.Warrant.Length(CheckDigitsLength); return _checkDigits; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get { Sentinel.Warrant.Length(CountryLength); return _countryCode; }
        }

        public bool Verified { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        // We only display the beginning of the IBAN value.
        private string DebuggerDisplay
            => _value.Length == MinLength
            ? _value
            : _value.Substring(0, MinLength) + "...";

        public static Iban Create(string countryCode, string checkDigits, string bban)
        {
            // REVIEW: We check for non-null twice...
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckCheckDigits(checkDigits), nameof(checkDigits));
            Require.True(CheckBban(bban), nameof(bban));

            var value = countryCode + checkDigits + bban;
            Contract.Assume(CheckValue(value));

            var iban = new Iban(countryCode, checkDigits, bban, value, false);
            if (!iban.CheckParts())
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }

            return iban;
        }

        public static Iban Parse(string value)
        {
            Expect.NotNull(value);

            return Parse(value, IbanStyles.Any);
        }

        public static Iban Parse(string value, IbanStyles styles)
        {
            Require.NotNull(value, nameof(value));

            var val = StripIgnorableSymbols(value, styles);
            if (!CheckValue(val))
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }
            Check.True(CheckValue(val));

            var iban = ParseCore(val, false);
            if (!iban.CheckParts())
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }

            return iban;
        }

        public static Iban ParseExact(string value)
        {
            Expect.NotNull(value);

            return ParseExact(value, IbanStyles.None);
        }

        public static Iban ParseExact(string value, IbanStyles styles)
        {
            Require.NotNull(value, nameof(value));

            var val = StripIgnorableSymbols(value, styles);
            if (!CheckValue(val))
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }
            Check.True(CheckValue(val));

            var iban = ParseCore(val, true);
            if (!iban.CheckParts())
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }
            if (!iban.Verify())
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return iban;
        }

        public static Iban? TryParse(string value) => TryParse(value, IbanStyles.Any);

        public static Iban? TryParse(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = StripIgnorableSymbols(value, styles);
            if (!CheckValue(val)) { return null; }
            Check.True(CheckValue(val));

            var iban = ParseCore(val, false);
            if (!iban.CheckParts()) { return null; }

            return iban;
        }

        public static Iban? TryParseExact(string value) => TryParseExact(value, IbanStyles.Any);

        public static Iban? TryParseExact(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = StripIgnorableSymbols(value, styles);
            if (!CheckValue(val)) { return null; }
            Check.True(CheckValue(val));

            var iban = ParseCore(val, true);
            if (!iban.CheckParts() && !iban.Verify()) { return null; }

            return iban;
        }

        public static Iban? Verify(Iban iban)
        {
            if (iban.Verified) { return iban; }
            if (!iban.Verify()) { return null; }

            return new Iban(iban.CountryCode, iban.CheckDigits, iban.Bban, iban._value, true);
        }

        internal bool Verify() => Verify(IbanVerificationModes.Integrity);

        private bool Verify(IbanVerificationModes modes)
        {
            throw new NotImplementedException();
        }

        private int SlowComputeChecksum()
        {
            //int len = _value.Length;
            //var tmp = new int[len];

            //for (var i = 0; i < len; i++)
            //{
            //    char ch = i < len - 4 ? _value[i + 4] : _value[(i + 4) % len];
            //    // If it is not a digit, it is an uppercase ASCII letter.
            //    // A is mapped to 10, B to 11, C to 12, and so on.
            //    tmp[i] = ch >= '0' && ch <= '9' ? ch - '0' : ch - 'A' + 10;
            //}

            throw new NotImplementedException();
        }

        public long FastComputeChecksum()
        {
            int len = _value.Length;
            long checksum = 0;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? _value[i + 4] : _value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum % 97;
        }

        internal bool CheckParts()
            // NB: We already checked the length for each property.
            => IsUpperLetter(CountryCode)
                && IsDigit(CheckDigits)
                && IsDigitOrUpperLetter(Bban);

        // NB: If "verified" is true, it does not mean that we have already performed any verification.
        //     The caller is in charge to do the right thing.
        internal static Iban ParseCore(string value, bool verified)
        {
            Demand.True(CheckValue(value));

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, CountryLength);
            Check.True(CheckCountryCode(countryCode));

            string checkDigits = value.Substring(CountryLength, CheckDigitsLength);
            Check.True(CheckCheckDigits(checkDigits));

            string bban = value.Substring(CountryLength + CheckDigitsLength);
            Contract.Assume(CheckBban(bban));

            return new Iban(countryCode, checkDigits, bban, value, verified);
        }

        private static string StripIgnorableSymbols(string input, IbanStyles styles)
        {
            Demand.NotNull(input);
            Warrant.NotNull<string>();

            if (input.Length == 0) { return String.Empty; }
            if (styles == IbanStyles.None) { return input; }

            var output = new char[input.Length];

            int len = input.Length;

            int start = 0;
            if (styles.Contains(IbanStyles.AllowLeadingWhite))
            {
                // Ignore leading whitespaces.
                while (start < len)
                {
                    if (input[start] != ' ') { break; }
                    start++;
                }
            }
            int end = len - 1;
            if (styles.Contains(IbanStyles.AllowTrailingWhite))
            {
                // Ignore trailing whitespaces.
                while (end >= 0)
                {
                    if (input[end] != ' ') { break; }
                    end--;
                }
            }

            bool rmspace = styles.Contains(IbanStyles.AllowInnerWhite);
            bool rmhyphen = styles.Contains(IbanStyles.AllowInnerHyphen);

            int k = 0;
            for (var i = start; i <= end; i++)
            {
                char ch = input[i];

                if (i != start && i != end)
                {
                    if (rmspace && ch == ' ') { continue; }
                    if (rmhyphen && ch == '-') { continue; }
                }

                output[k] = ch;
                k++;
            }

            return new String(output, 0, k);
        }
    }

    // Implements the IFormattable interface.
    public partial struct Iban
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

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (formatProvider != null)
            {
                var fmt = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (fmt != null)
                {
                    return fmt.Format(format, this, formatProvider);
                }
            }

            if (format == null || format.Length == 0)
            {
                format = DefaultFormat;
            }
            else if (format.Length != 1)
            {
                throw new FormatException(
                    Narvalo.Format.Current(Strings.Iban_InvalidFormatSpecification));
            }

            switch (format)
            {
                case "G":
                case "g":
                    // General (default): insert a space every 4 chars.
                    return Format(_value, ' ');
                case "D":
                case "d":
                    // Display: insert an hyphen every 4 chars.
                    return Format(_value, '-');
                case "N":
                case "n":
                    // Neutral.
                    return _value;
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Iban_InvalidFormatSpecification));
            }
        }

        private static string Format(string input, char ch)
        {
            Demand.NotNull(input);
            Warrant.NotNull<string>();

            int len = input.Length;

            int r = len % 4;
            int q = (len - r) / 4;
            int outlen = len + q - (r == 0 ? 1 : 0);
            var output = new char[outlen];

            int k = 1;
            for (var i = 0; i < len; i++, k++)
            {
                if (k % 5 == 0)
                {
                    output[k - 1] = ch;
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

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right) => left.Equals(right);

        public static bool operator !=(Iban left, Iban right) => !left.Equals(right);

        public bool Equals(Iban other) => _value == other._value && Verified == other.Verified;

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Utilities.IbanFormat;

    public partial struct Iban
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(CheckBban(_bban));
            Contract.Invariant(CheckCheckDigits(_checkDigits));
            Contract.Invariant(CheckCountryCode(_countryCode));
            Contract.Invariant(CheckValue(_value));
        }
    }
}

#endif
