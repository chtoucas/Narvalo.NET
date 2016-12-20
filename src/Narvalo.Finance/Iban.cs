// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Validation;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.Validation.AsciiHelpers;
    using static Narvalo.Finance.Validation.IbanFormat;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Iban : IEquatable<Iban>, IFormattable
    {
        private const string DEFAULT_FORMAT = "D";
        private const char WHITESPACE_CHAR = ' ';
        private const int CHECKSUM_MODULO = 97;

        private readonly string _bban;
        private readonly string _checkDigits;
        private readonly string _countryCode;
        private readonly string _value;

        private Iban(
            string countryCode,
            string checkDigits,
            string bban,
            string value,
            bool integrityChecked)
        {
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckCheckDigits(checkDigits));
            Demand.True(CheckBban(bban));
            Demand.True(CheckValue(value));

            _countryCode = countryCode;
            _checkDigits = checkDigits;
            _bban = bban;
            _value = value;
            IntegrityChecked = integrityChecked;
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

        public bool IntegrityChecked { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        // We only display the beginning of the IBAN value.
        private string DebuggerDisplay
            => _value.Length == MinLength
            ? _value
            : _value.Substring(0, MinLength) + "...";

        public static Iban Create(string countryCode, string checkDigits, string bban)
        {
            Expect.True(CheckCountryCode(countryCode));
            Expect.True(CheckCheckDigits(checkDigits));
            Expect.True(CheckBban(bban));

            return Create(countryCode, checkDigits, bban, true);
        }

        public static Iban Create(string countryCode, string checkDigits, string bban, bool checkIntegrity)
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

            var parts = new IbanParts
            {
                Bban = bban,
                CheckDigits = checkDigits,
                CountryCode = countryCode,
            };

            if (!parts.Check())
            {
                throw new FormatException(Strings.Iban_BadPartsFormat);
            }

            // WARNING: This MUST stay after CheckValue() and parts.Check(). See CheckIntegrity().
            if (checkIntegrity && !CheckIntegrity(value))
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return new Iban(parts.CountryCode, parts.CheckDigits, parts.Bban, value, checkIntegrity);
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

            var parts = IbanParts.Create(val);
            if (!parts.Check())
            {
                throw new FormatException(Strings.Iban_BadPartsFormat);
            }

            return new Iban(parts.CountryCode, parts.CheckDigits, parts.Bban, val, false);
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

            var parts = IbanParts.Create(val);
            if (!parts.Check())
            {
                throw new FormatException(Strings.Iban_BadPartsFormat);
            }

            // WARNING: This MUST stay after CheckValue() and parts.Check(). See CheckIntegrity().
            if (!CheckIntegrity(val))
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return new Iban(parts.CountryCode, parts.CheckDigits, parts.Bban, val, true);
        }

        public static Iban? TryParse(string value) => TryParse(value, IbanStyles.Any);

        public static Iban? TryParse(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = StripIgnorableSymbols(value, styles);
            if (!CheckValue(val)) { return null; }
            Check.True(CheckValue(val));

            var parts = IbanParts.Create(val);
            if (!parts.Check()) { return null; }

            return new Iban(parts.CountryCode, parts.CheckDigits, parts.Bban, val, false);
        }

        public static Iban? TryParseExact(string value) => TryParseExact(value, IbanStyles.Any);

        public static Iban? TryParseExact(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = StripIgnorableSymbols(value, styles);
            if (!CheckValue(val)) { return null; }
            Check.True(CheckValue(val));

            var parts = IbanParts.Create(val);
            if (!parts.Check()) { return null; }

            // WARNING: This MUST stay after CheckValue() and parts.Check(). See CheckIntegrity().
            if (!CheckIntegrity(val)) { return null; }

            return new Iban(parts.CountryCode, parts.CheckDigits, parts.Bban, val, true);
        }

        public static Iban? CheckIntegrity(Iban iban)
        {
            if (iban.IntegrityChecked) { return iban; }
            // It is safe to call CheckIntegrity() since "_value" is guaranteed to be valid. See CheckIntegrity().
            if (!CheckIntegrity(iban._value)) { return null; }

            return new Iban(iban.CountryCode, iban.CheckDigits, iban.Bban, iban._value, true);
        }

        // We only verify the integrity of the whole IBAN; we do not validate the BBAN part.
        // The algorithm is as follows:
        // 1. Move the first 4 chars to the end of the value.
        // 2. Replace '0' by 0, '1' by 1, etc.
        // 3. Replace 'A' by 10, 'B' by 11, etc.
        // 4. Verify that the resultint integer modulo 97 is equal to 1.
        // NB: Only works if you can ensure that "value" is only made up of alphanumeric
        // ASCII characters. Here this means that we have called CheckValue() and parts.Check().
        internal static bool CheckIntegrity(string value)
        {
            Demand.True(CheckValue(value));

            // NB: On full .NET we have Environment.Is64BitProcess.
            // If IntPtr.Size is equal to 8, we are running in a 64-bit process and
            // we check the integrity using Int64 arithmetic; otherwize (32-bit or 16-bit process)
            // we use Int32 arithmetic (NB: IntPtr.Size = 4 in a 32-bit process). I believe,
            // but I have not verified, that ComputeInt64Checksum() is faster for a 64-bit processor.
            return CheckIntegrity(value, IntPtr.Size == 8);
        }

        internal static bool CheckIntegrity(string value, bool sixtyfour)
        {
            Demand.True(CheckValue(value));

            return sixtyfour
                ? ComputeInt64Checksum(value) % CHECKSUM_MODULO == 1
                : ComputeInt32Checksum(value) % CHECKSUM_MODULO == 1;
        }

        internal struct IbanParts
        {
            public string Bban { get; set; }
            public string CheckDigits { get; set; }
            public string CountryCode { get; set; }

            public static IbanParts Create(string value)
            {
                Demand.True(CheckValue(value));

                // The first two letters define the ISO 3166-1 alpha-2 country code.
                string countryCode = value.Substring(0, CountryLength);
                Narvalo.Check.True(CheckCountryCode(countryCode));

                string checkDigits = value.Substring(CountryLength, CheckDigitsLength);
                Narvalo.Check.True(CheckCheckDigits(checkDigits));

                string bban = value.Substring(CountryLength + CheckDigitsLength);
                Contract.Assume(CheckBban(bban));

                return new IbanParts
                {
                    Bban = bban,
                    CheckDigits = checkDigits,
                    CountryCode = countryCode,
                };
            }

            // NB: We already checked the length for each property.
            public bool Check()
                => CountryISOCodes.TwoLetterCodeExists(CountryCode)
                && IsDigit(CheckDigits)
                && IsDigitOrUpperLetter(Bban);
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
                    if (input[start] != WHITESPACE_CHAR) { break; }
                    start++;
                }
            }
            int end = len - 1;
            if (styles.Contains(IbanStyles.AllowTrailingWhite))
            {
                // Ignore trailing whitespaces.
                while (end >= 0)
                {
                    if (input[end] != WHITESPACE_CHAR) { break; }
                    end--;
                }
            }

            bool rmspace = styles.Contains(IbanStyles.AllowInnerWhite);

            int k = 0;
            for (var i = start; i <= end; i++)
            {
                char ch = input[i];

                // Normally, there is either a single whitespace char every four chars
                // or no whitespace char at all. Here, we don't bother and just ignore them.
                if (rmspace && ch == WHITESPACE_CHAR && i != start && i != end) { continue; }

                output[k] = ch;
                k++;
            }

            return new String(output, 0, k);
        }

        private static int ComputeInt32Checksum(string value)
        {
            Demand.True(CheckValue(value));

            const int MAX_DIGIT = (Int32.MaxValue - 9) / 10;
            const int MAX_LETTER = (Int32.MaxValue - 35) / 100;

            int len = value.Length;
            int checksum = 0;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum;
        }

        private static long ComputeInt64Checksum(string value)
        {
            Demand.True(CheckValue(value));

            const long MAX_DIGIT = (Int64.MaxValue - 9) / 10;
            const long MAX_LETTER = (Int64.MaxValue - 35) / 100;

            int len = value.Length;
            long checksum = 0L;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum;
        }
    }

    // Implements the IFormattable interface.
    public partial struct Iban
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();

            return ToString(DEFAULT_FORMAT, null);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();

            return ToString(format, null);
        }

        // NB: We ignore any user supplied "formatProvider".
        public string ToString(string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "D":
                case "d":
                    // Display (default): insert a whitespace char every 4 chars.
                    // This format is NOT suitable for electronic transmission.
                    return FormatD(_value);
                case "G":
                case "g":
                    // General.
                    return _value;
                default:
                    throw new FormatException(Format.Current(Strings.Iban_InvalidFormatSpecification));
            }
        }

        private static string FormatD(string input)
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

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right) => left.Equals(right);

        public static bool operator !=(Iban left, Iban right) => !left.Equals(right);

        public bool Equals(Iban other)
            => _value == other._value && IntegrityChecked == other.IntegrityChecked;

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode() => _value.GetHashCode() ^ IntegrityChecked.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Validation.IbanFormat;

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
