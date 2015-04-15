// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif
    using System.Linq;

    /// <summary>
    /// Represents a Business Identifier Code.
    /// It was previously understood to be an acronym for Bank Identifier Code.
    /// </summary>
    /// <remarks>
    /// The standard format for a Business Identifier Code is defined in ISO 9362.
    /// </remarks>
    public partial struct Bic : IEquatable<Bic>
    {
        public const string PrimaryOfficeBranchCode = "XXX";

        private readonly string _branchCode;
        private readonly string _countryCode;
        private readonly string _institutionCode;
        private readonly string _locationCode;

        Bic(
           string institutionCode,
           string countryCode,
           string locationCode,
           string branchCode)
        {
            _institutionCode = institutionCode;
            _countryCode = countryCode;
            _locationCode = locationCode;
            _branchCode = branchCode;
        }

        /// <summary>
        /// Gets the branch code.
        /// </summary>
        public string BranchCode { get { return _branchCode; } }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode { get { return _countryCode; } }

        /// <summary>
        /// Gets the institution code or bank code.
        /// </summary>
        public string InstitutionCode { get { return _institutionCode; } }

        public bool IsPrimaryOfficeBic
        {
            get { return BranchCode == PrimaryOfficeBranchCode || BranchCode.Length == 0; }
        }

        /// <summary>
        /// Gets the location code.
        /// </summary>
        public string LocationCode { get { return _locationCode; } }

        public static Bic Parse(string value)
        {
            Require.NotNullOrEmpty(value, "value");

            return ParseCore_(value, true /* throwOnError */).Value;
        }

        public static Bic? TryParse(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ParseCore_(value, false /* throwOnError */);
        }

        public override string ToString()
        {
            return _institutionCode + _countryCode + _locationCode + _branchCode;
        }

        private static Bic? ParseCore_(string value, bool throwOnError)
        {
            if (value.Length != 8 && value.Length != 11)
            {
                if (throwOnError)
                {
                    throw new ArgumentException("The BIC string MUST be 8 or 11 characters long.", value);
                }
                else
                {
                    return null;
                }
            }

            // The first four letters define the institution or bank code.
            string institutionCode = value.Substring(0, 4);
            if (!institutionCode.ToCharArray().All(c => { var pos = (int)c; return pos >= 65 && pos <= 90; }))
            {
                if (throwOnError)
                {
                    throw new ArgumentException(
                        "The first 4 characters of a BIC string MUST be made up of ASCII uppercase letters.",
                        value);
                }
                else
                {
                    return null;
                }
            }

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(4, 2);

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(6, 2);

            // The next 3 letters or digits define the branch code (optional).
            string branchCode = value.Length == 11 ? value.Substring(8, 3) : String.Empty;

            return new Bic(institutionCode, countryCode, locationCode, branchCode);
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_branchCode != null);
            Contract.Invariant(_countryCode != null);
            Contract.Invariant(_institutionCode != null);
            Contract.Invariant(_locationCode != null);
        }

#endif
    }

    // Implements the IEquatable<Bic> interface.
    public partial struct Bic
    {
        public static bool operator ==(Bic left, Bic right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Bic left, Bic right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Bic other)
        {
            return _institutionCode == other._institutionCode
                && _countryCode == other._countryCode
                && _locationCode == other._locationCode
                && _branchCode == other._branchCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Bic))
            {
                return false;
            }

            return Equals((Bic)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _institutionCode.GetHashCode();
                hash = hash * 23 + _countryCode.GetHashCode();
                hash = hash * 23 + _locationCode.GetHashCode();
                hash = hash * 23 + _branchCode.GetHashCode();

                return hash;
            }
        }
    }
}
