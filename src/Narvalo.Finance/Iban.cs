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

        private readonly IbanParts _parts;

        private Iban(IbanParts parts, bool integrityChecked)
        {
            _parts = parts;
            IntegrityChecked = integrityChecked;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get { Warrant.NotNull<string>(); return _parts.Bban; }
        }

        /// <summary>
        /// Gets the check digits.
        /// </summary>
        public string CheckDigits
        {
            get { Warrant.NotNull<string>(); return _parts.CheckDigits; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get { Warrant.NotNull<string>(); return _parts.CountryCode; }
        }

        public bool IntegrityChecked { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        // We only display the beginning of the IBAN value.
        private string DebuggerDisplay
            => _parts.LiteralValue.Length > IbanParts.MinLength
            ? _parts.LiteralValue.Substring(0, IbanParts.MinLength) + "..."
            : _parts.LiteralValue;

        public static Iban Create(string countryCode, string checkDigits, string bban)
        {
            Expect.NotNull(countryCode);
            Expect.NotNull(checkDigits);
            Expect.NotNull(bban);

            return Create(countryCode, checkDigits, bban, IbanValidations.Integrity);
        }

        public static Iban Create(string countryCode, string checkDigits, string bban, IbanValidations validations)
        {
            Expect.NotNull(countryCode);
            Expect.NotNull(checkDigits);
            Expect.NotNull(bban);

            var parts = IbanParts.Create(countryCode, checkDigits, bban);

            if (validations.Contains(IbanValidations.ISOCountryCode)
                && !IbanValidator.CheckCountryCode(parts))
            {
                throw new FormatException(Strings.Iban_UnknownISOCountryCode);
            }

            bool checkIntegrity = validations.Contains(IbanValidations.Integrity);
            if (checkIntegrity && !IbanValidator.CheckIntegrity(parts))
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return new Iban(parts, checkIntegrity);
        }

        public static Iban Parse(string value)
        {
            Expect.NotNull(value);

            return Parse(value, IbanStyles.None, IbanValidations.Integrity);
        }

        public static Iban Parse(string value, IbanStyles styles)
        {
            Expect.NotNull(value);

            return Parse(value, styles, IbanValidations.Integrity);
        }

        public static Iban Parse(string value, IbanValidations validations)
        {
            Expect.NotNull(value);

            return Parse(value, IbanStyles.None, validations);
        }

        public static Iban Parse(string value, IbanStyles styles, IbanValidations validations)
        {
            Require.NotNull(value, nameof(value));

            var val = StripIgnorableSymbols(value, styles);

            var parts = IbanParts.Parse(val);

            if (validations.Contains(IbanValidations.ISOCountryCode)
                && !IbanValidator.CheckCountryCode(parts))
            {
                throw new FormatException(Strings.Iban_UnknownISOCountryCode);
            }

            bool checkIntegrity = validations.Contains(IbanValidations.Integrity);
            if (checkIntegrity && !IbanValidator.CheckIntegrity(parts))
            {
                throw new FormatException(Strings.Iban_IntegrityCheckFailure);
            }

            return new Iban(parts, checkIntegrity);
        }

        public static Iban? TryParse(string value)
            => TryParse(value, IbanStyles.None, IbanValidations.Integrity);

        public static Iban? TryParse(string value, IbanStyles styles)
            => TryParse(value, styles, IbanValidations.Integrity);

        public static Iban? TryParse(string value, IbanValidations validations)
            => TryParse(value, IbanStyles.None, validations);

        public static Iban? TryParse(string value, IbanStyles styles, IbanValidations validations)
        {
            if (value == null) { return null; }

            var val = StripIgnorableSymbols(value, styles);

            var parts = IbanParts.TryParse(val);
            if (!parts.HasValue) { return null; }

            if (validations.Contains(IbanValidations.ISOCountryCode)
                && !IbanValidator.CheckCountryCode(parts.Value))
            {
                return null;
            }

            bool checkIntegrity = validations.Contains(IbanValidations.Integrity);
            if (checkIntegrity && !IbanValidator.CheckIntegrity(parts.Value)) { return null; }

            return new Iban(parts.Value, checkIntegrity);
        }

        public static Iban? CheckIntegrity(Iban iban)
        {
            if (iban.IntegrityChecked) { return iban; }
            // It is safe to call CheckIntegrity() since "_value" is guaranteed to be valid. See CheckIntegrity().
            if (!IbanValidator.CheckIntegrity(iban._parts)) { return null; }

            return new Iban(iban._parts, true);
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
                    return FormatD(_parts.LiteralValue);
                case "H":
                case "h":
                    // Human: same as "D" but prefixed with "IBAN ".
                    // This format is NOT suitable for electronic transmission.
                    return HumanHeader + FormatD(_parts.LiteralValue);
                case "G":
                case "g":
                    // General.
                    return _parts.LiteralValue;
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
            => _parts == other._parts && IntegrityChecked == other.IntegrityChecked;

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode() => _parts.GetHashCode() ^ IntegrityChecked.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Text.IbanParts;

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
