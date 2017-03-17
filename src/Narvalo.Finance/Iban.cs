// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Applicative;
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
        private const char WHITESPACE_CHAR = ' ';

        private readonly IbanParts _parts;

        private Iban(IbanParts parts, IbanValidationLevels levels)
        {
            _parts = parts;
            VerificationLevels = levels;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban => _parts.Bban;

        /// <summary>
        /// Gets the check digits.
        /// </summary>
        public string CheckDigits => _parts.CheckDigits;

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode => _parts.CountryCode;

        public IbanValidationLevels VerificationLevels { get; }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        // We only display the beginning of the IBAN value.
        private string DebuggerDisplay
            => _parts.LiteralValue.Length > IbanParts.MinLength
            ? _parts.LiteralValue.Substring(0, IbanParts.MinLength) + "..."
            : _parts.LiteralValue;

        // REVIEW: TryBuild? Add validation?
        public static Iban Build(string countryCode, string bban)
        {
            var parts = IbanParts.Build(countryCode, bban);

            return new Iban(parts, IbanValidationLevels.Integrity);
        }

        public static Iban Create(string countryCode, string checkDigits, string bban)
            => Create(countryCode, checkDigits, bban, IbanValidationLevels.Default);

        public static Iban Create(string countryCode, string checkDigits, string bban, IbanValidationLevels levels)
        {
            var parts = IbanParts.Create(countryCode, checkDigits, bban);

            IbanValidator.TryValidate(parts, levels)
                .OnError(err => { throw new ArgumentException(err); });

            return new Iban(parts, levels);
        }

        public static Iban? Parse(string value)
            => Parse(value, IbanStyles.None, IbanValidationLevels.Default);

        public static Iban? Parse(string value, IbanStyles styles)
            => Parse(value, styles, IbanValidationLevels.Default);

        public static Iban? Parse(string value, IbanValidationLevels levels)
            => Parse(value, IbanStyles.None, levels);

        public static Iban? Parse(string value, IbanStyles styles, IbanValidationLevels levels)
        {
            if (value == null) { return null; }

            var val = PreprocessInput(value, styles);

            var parts = IbanParts.Parse(val);
            if (!parts.HasValue) { return null; }

            if (!IbanValidator.Validate(parts.Value, levels)) { return null; }

            return new Iban(parts.Value, levels);
        }

        public static Outcome<Iban> TryParse(string value)
            => TryParse(value, IbanStyles.None, IbanValidationLevels.Default);

        public static Outcome<Iban> TryParse(string value, IbanStyles styles)
            => TryParse(value, styles, IbanValidationLevels.Default);

        public static Outcome<Iban> TryParse(string value, IbanValidationLevels levels)
            => TryParse(value, IbanStyles.None, levels);

        public static Outcome<Iban> TryParse(string value, IbanStyles styles, IbanValidationLevels levels)
        {
            if (value == null)
            {
                return Outcome<Iban>.FromError(Format.Current(Strings.InvalidIbanValue_Format, value));
            }

            string input = PreprocessInput(value, styles);

            return from val in IbanParts.TryParse(input)
                   where IbanValidator.TryValidate(val, levels)
                   select new Iban(val, levels);
        }

        public static Iban? CheckIntegrity(Iban iban)
        {
            if (iban.VerificationLevels.Contains(IbanValidationLevels.Integrity)) { return iban; }
            if (!IbanValidator.VerifyIntegrity(iban._parts)) { return null; }

            return new Iban(iban._parts, iban.VerificationLevels | IbanValidationLevels.Integrity);
        }

        // NB: Normally, there is either a single whitespace char every four chars or no whitespace
        // char at all. Here we are more permissive: multiple whitespaces are ok, and position check
        // is not enforced.
        private static string PreprocessInput(string text, IbanStyles styles)
        {
            Debug.Assert(text != null);

            // Fast track.
            if (styles == IbanStyles.None) { return text; }
            if (text.Length == 0) { return String.Empty; }

            int len = text.Length;

            int start = 0;
            if (styles.Contains(IbanStyles.AllowLeadingWhite))
            {
                // Ignore leading whitespaces.
                while (start < len)
                {
                    if (text[start] != WHITESPACE_CHAR) { break; }
                    start++;
                }
            }
            // The input contains only whitespaces.
            if (start == len) { return String.Empty; }

            int end = len - 1;
            if (styles.Contains(IbanStyles.AllowTrailingWhite))
            {
                // Ignore trailing whitespaces.
                while (end > start)
                {
                    if (text[end] != WHITESPACE_CHAR) { break; }
                    end--;
                }
            }

            if (styles.Contains(IbanStyles.AllowHeader)
                && text.Length >= start + 5
                && text.Substring(start, 5) == IbanParts.HumanHeader)
            {
                start += 5;
            }

            bool removespaces = styles.Contains(IbanStyles.AllowInnerWhite);
            bool transformcase = styles.Contains(IbanStyles.AllowLowercaseLetter);

            // NB: If end - start + 1 < MinLength, the input is clearly invalid, nevertheless
            // we continue to process the input until we completely fulfill the method's contract:
            // cleanup the input according to the user provided rules.
            var output = new char[end - start + 1];

            int k = 0;
            for (var i = start; i <= end; i++)
            {
                char ch = text[i];

                // NB: If IbanStyles.AllowIbanPrefix is on and we did remove an IBAN prefix,
                // we might have whitespaces again at the beginning of the loop. Lesson: do not
                // exclude i = start. We do not exclude i = end too, even though we already know
                // that "ch" is not a whitespace; useless optimization.
                if (removespaces && ch == WHITESPACE_CHAR) { continue; }

                if (transformcase && ch >= 'a' && ch <= 'z')
                {
                    // NB: We use a known property of the ASCII charset; for instance
                    //       01100001 = 0x61 = 'a'
                    //   AND 11011111 = 0xDF = ~0x20
                    //       --------
                    //       01000001 = 0x41 = 'A'
                    output[k] = (char)(ch & 0xDF);
                }
                else
                {
                    output[k] = ch;
                }
                k++;
            }

            return new String(output, 0, k);
        }
    }

    // Implements the IFormattable interface.
    public partial struct Iban
    {
        public override string ToString() => _parts.ToString(IbanParts.DefaultFormat, null);

        public string ToString(string format) => _parts.ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider)
            => _parts.ToString(format, formatProvider);
    }

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right) => left.Equals(right);

        public static bool operator !=(Iban left, Iban right) => !left.Equals(right);

        public bool Equals(Iban other)
            => _parts == other._parts && VerificationLevels == other.VerificationLevels;

        public override bool Equals(object obj) => (obj is Iban) && Equals((Iban)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(_parts.GetHashCode(), VerificationLevels.GetHashCode());
    }
}
