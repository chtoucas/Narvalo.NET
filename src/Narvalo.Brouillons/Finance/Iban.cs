// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif
    using System.Linq;

    /// <summary>
    /// Represents an International Bank Account Number.
    /// </summary>
    public partial struct Iban : IEquatable<Iban>
    {
        private readonly string _bban;
        private readonly string _checkDigit;
        private readonly string _countryCode;

        Iban(
           string countryCode,
           string checkDigit,
           string bban)
        {
            _countryCode = countryCode;
            _checkDigit = checkDigit;
            _bban = bban;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban { get { return _bban; } }

        /// <summary>
        /// Gets the check digit.
        /// </summary>
        public string CheckDigit { get { return _checkDigit; } }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode { get { return _countryCode; } }

        public bool Check()
        {
            string iban = ToString();
            int checksum = 0;
            int length = iban.Length;

            for (int i = 0; i < length; i++)
            {
                //if (iban[i] == ' ')
                //{
                //    continue;
                //}

                int value;
                char c = iban[(i + 4) % length];

                if ((c >= '0') && (c <= '9'))
                {
                    value = c - '0';
                }
                else if ((c >= 'A') && (c <= 'Z'))
                {
                    value = c - 'A';
                    checksum = (checksum * 10 + (1 + value / 10)) % 97;
                    value %= 10;
                }
                else if ((c >= 'a') && (c <= 'z'))
                {
                    value = c - 'a';
                    checksum = (checksum * 10 + (1 + value / 10)) % 97;
                    value %= 10;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                checksum = (checksum * 10 + value) % 97;
            }

            return checksum == 1;
        }

        public static Iban Parse(string value)
        {
            Require.NotNullOrEmpty(value, "value");

            return ParseCore_(value, true /* throwOnError */).Value;
        }

        public static Iban? TryParse(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ParseCore_(value, false /* throwOnError */);
        }

        public override string ToString()
        {
            // XXX
            return _countryCode + _checkDigit + _bban;
        }

        private static Iban? ParseCore_(string value, bool throwOnError)
        {
            if (value.Length < 14 || value.Length > 34)
            {
                if (throwOnError)
                {
                    throw new ArgumentException(
                        "The IBAN string MUST be at most 34 characters long and at least 14 characters long.",
                        value);
                }
                else
                {
                    return null;
                }
            }
            if (!value.ToCharArray().All(c => { var pos = (int)c; return (pos >= 48 && pos <= 57) || (pos >= 65 && pos <= 90); }))
            {
                if (throwOnError)
                {
                    throw new ArgumentException(
                        "The IBAN string MUST only contains digits and ASCII uppercase letters.",
                        value);
                }
                else
                {
                    return null;
                }
            }

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, 2);

            string checkDigit = value.Substring(2, 2);

            string bban = value.Substring(4);

            return new Iban(countryCode, checkDigit, bban);
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_bban != null);
            Contract.Invariant(_checkDigit != null);
            Contract.Invariant(_countryCode != null);
        }

#endif
    }

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Iban left, Iban right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Iban other)
        {
            return _countryCode == other._countryCode
                && _checkDigit == other._checkDigit
                && _bban == other._bban;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _countryCode.GetHashCode();
                hash = hash * 23 + _checkDigit.GetHashCode();
                hash = hash * 23 + _bban.GetHashCode();

                return hash;
            }
        }
    }
}
