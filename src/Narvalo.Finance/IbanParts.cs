// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    using static Narvalo.Finance.Utilities.AsciiHelpers;

    public partial struct IbanParts : IEquatable<IbanParts>
    {
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

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

        public static IbanParts Create(string countryCode, string checkDigits, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));

            if (CountryPart.Validate(countryCode)
                && CheckDigitsPart.Validate(checkDigits)
                && BbanPart.Validate(bban))
            {
                return new IbanParts(countryCode, checkDigits, bban);
            }
            else
            {
                throw new ArgumentException(Strings.Argument_InvalidIbanParts);
            }
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

        private static bool CheckLength(string value)
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

            private static bool CheckContent(string value) => IsDigitOrUpperLetter(value);
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

            private static bool CheckContent(string value) => IsDigit(value);
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

            private static bool CheckContent(string value) => IsUpperLetter(value);
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
