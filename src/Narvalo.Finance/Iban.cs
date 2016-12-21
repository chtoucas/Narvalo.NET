// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Finance.Text;
    using Narvalo.Finance.Validation;
    using Narvalo.Finance.Properties;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Iban : IEquatable<Iban>, IFormattable
    {
        public const string HumanHeader = "IBAN ";

        private const string DEFAULT_FORMAT = "D";
        private const char WHITESPACE_CHAR = ' ';
        private const int CHECKSUM_MODULO = 97;

        private readonly IbanParts _parts;
        private readonly string _value;

        private Iban(IbanParts parts, string value, bool integrityChecked)
        {
            Demand.NotNull(value);

            _parts = parts;
            _value = value;
            IntegrityChecked = integrityChecked;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get { Sentinel.Warrant.LengthRange(IbanParts.BbanMinLength, IbanParts.BbanMaxLength); return _parts.Bban; }
        }

        /// <summary>
        /// Gets the check digits.
        /// </summary>
        public string CheckDigits
        {
            get { Sentinel.Warrant.Length(IbanParts.CheckDigitsLength); return _parts.CheckDigits; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get { Sentinel.Warrant.Length(IbanParts.CountryLength); return _parts.CountryCode; }
        }

        public bool IntegrityChecked { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        // We only display the beginning of the IBAN value.
        private string DebuggerDisplay
            => _value.Length == IbanParts.MinLength
            ? _value
            : _value.Substring(0, IbanParts.MinLength) + "...";

        public static Iban Create(string countryCode, string checkDigits, string bban)
        {
            Expect.NotNull(countryCode);
            Expect.NotNull(checkDigits);
            Expect.NotNull(bban);

            return Create(countryCode, checkDigits, bban, true);
        }

        public static Iban Create(string countryCode, string checkDigits, string bban, bool checkIntegrity)
        {
            Expect.NotNull(countryCode);
            Expect.NotNull(checkDigits);
            Expect.NotNull(bban);

            var parts = IbanParts.Create(countryCode, checkDigits, bban);
            if (!parts.HasValue)
            {
                throw new FormatException(Strings.Iban_BadPartsFormat);
            }

            var value = countryCode + checkDigits + bban;

            // It is safe to call CheckIntegrity() since "value" is guaranteed to be valid. See CheckIntegrity().
            if (checkIntegrity && !CheckIntegrity(value))
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return new Iban(parts.Value, value, checkIntegrity);
        }

        public static Iban Parse(string value)
        {
            Expect.NotNull(value);

            return Parse(value, IbanStyles.None);
        }

        public static Iban Parse(string value, IbanStyles styles)
        {
            Require.NotNull(value, nameof(value));

            var val = StripIgnorableSymbols(value, styles);

            var parts = IbanParts.Parse(val);
            if (!parts.HasValue)
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }

            if (styles.Contains(IbanStyles.CheckISOCountryCode)
                && !CountryISOCodes.TwoLetterCodeExists(parts.Value.CountryCode))
            {
                throw new FormatException(Strings.Iban_UnknownISOCountryCode);
            }

            bool checkIntegrity = styles.Contains(IbanStyles.CheckIntegrity);
            // It is safe to call CheckIntegrity() since "val" is guaranteed to be valid. See CheckIntegrity().
            if (checkIntegrity && !CheckIntegrity(val))
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return new Iban(parts.Value, val, checkIntegrity);
        }

        public static Iban? TryParse(string value) => TryParse(value, IbanStyles.None);

        public static Iban? TryParse(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = StripIgnorableSymbols(value, styles);

            var parts = IbanParts.Parse(val);
            if (!parts.HasValue) { return null; }

            if (styles.Contains(IbanStyles.CheckISOCountryCode)
                && !CountryISOCodes.TwoLetterCodeExists(parts.Value.CountryCode))
            {
                return null;
            }

            bool checkIntegrity = styles.Contains(IbanStyles.CheckIntegrity);
            // It is safe to call CheckIntegrity() since "val" is guaranteed to be valid. See CheckIntegrity().
            if (checkIntegrity && !CheckIntegrity(val)) { return null; }

            return new Iban(parts.Value, val, checkIntegrity);
        }

        public static Iban? CheckIntegrity(Iban iban)
        {
            if (iban.IntegrityChecked) { return iban; }
            // It is safe to call CheckIntegrity() since "_value" is guaranteed to be valid. See CheckIntegrity().
            if (!CheckIntegrity(iban._value)) { return null; }

            return new Iban(iban._parts, iban._value, true);
        }

        // We only verify the integrity of the whole IBAN; we do not validate the BBAN.
        // The algorithm is as follows:
        // 1. Move the leading 4 chars to the end of the value.
        // 2. Replace '0' by 0, '1' by 1, etc.
        // 3. Replace 'A' by 10, 'B' by 11, etc.
        // 4. Verify that the resulting integer modulo 97 is equal to 1.
        // WARNING: Only works if you can ensure that "value" is only made up of alphanumeric
        // ASCII characters. Here this means that we have already called CheckValue()
        // and parts.Check().
        internal static bool CheckIntegrity(string value)
        {
            Demand.NotNull(value);

            // NB: On full .NET we have Environment.Is64BitProcess.
            // If IntPtr.Size is equal to 8, we are running in a 64-bit process and
            // we check the integrity using Int64 arithmetic; otherwize (32-bit or 16-bit process)
            // we use Int32 arithmetic (NB: IntPtr.Size = 4 in a 32-bit process). I believe,
            // but I have not verified, that ComputeInt64Checksum() is faster for a 64-bit processor.
            return CheckIntegrity(value, IntPtr.Size == 8);
        }

        internal static bool CheckIntegrity(string value, bool sixtyfour)
        {
            Demand.NotNull(value);

            return sixtyfour
                ? ComputeInt64Checksum(value) % CHECKSUM_MODULO == 1
                : ComputeInt32Checksum(value) % CHECKSUM_MODULO == 1;
        }

        // NB: Normally, there is either a single whitespace char every four chars or no whitespace
        // char at all. Here we are more permissive: multiple whitespaces are ok, and position check
        // is not enforced.
        private static string StripIgnorableSymbols(string input, IbanStyles styles)
        {
            Demand.NotNull(input);
            Warrant.NotNull<string>();

            if (input.Length == 0) { return String.Empty; }
            if (styles == IbanStyles.None) { return input; }

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
            // The input contains only whitespaces.
            if (start == len) { return String.Empty; }

            int end = len - 1;
            if (styles.Contains(IbanStyles.AllowTrailingWhite))
            {
                // Ignore trailing whitespaces.
                // NB: We know for sure that end > start; otherwise input would have been an empty
                // string but we already handled this case above.
                while (end > start)
                {
                    if (input[end] != WHITESPACE_CHAR) { break; }
                    end--;
                }
            }

            if (styles.Contains(IbanStyles.AllowHeader)
                && input.Length >= start + 5
                && input.Substring(start, 5) == HumanHeader)
            {
                start += 5;
            }

            // FIXME: Use AllowLowercaseLetter.
            bool rmspace = styles.Contains(IbanStyles.AllowInnerWhite);

            var output = new char[end - start + 1];

            int k = 0;
            for (var i = start; i <= end; i++)
            {
                char ch = input[i];

                // NB: If IbanStyles.AllowIbanPrefix is on and we did remove an IBAN prefix,
                // we might have whitespaces again at the beginning of the loop. Lesson: do not
                // exclude i = start. We do not exclude i = end, even though we know that "ch" is
                // not a whitespace, only because this does not change anything.
                if (rmspace && ch == WHITESPACE_CHAR) { continue; }

                output[k] = ch;
                k++;
            }

            return new String(output, 0, k);
        }

        private static int ComputeInt32Checksum(string value)
        {
            Demand.NotNull(value);

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
            Demand.NotNull(value);

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
                case "H":
                case "h":
                    // Human: same as "D" but prefixed with "IBAN ".
                    // This format is NOT suitable for electronic transmission.
                    return HumanHeader + FormatD(_value);
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

    using static Narvalo.Finance.Validation.IbanParts;

    public partial struct Iban
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_value != null);
        }
    }
}

#endif
