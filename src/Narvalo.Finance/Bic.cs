// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Internal.Helpers;

    /// <summary>
    /// Represents a Business Identifier Code (BIC).
    /// </summary>
    /// <remarks>
    /// It was previously understood to be an acronym for Bank Identifier Code.
    /// The standard format for a BIC is defined in ISO 9362.
    /// </remarks>
    public partial struct Bic : IEquatable<Bic>
    {
        public const string PrimaryOfficeBranchCode = "XXX";

        private readonly string _value;

        //public Bic(string institutionCode, string countryCode, string locationCode, string branchCode)
        //    : this(
        //          institutionCode,
        //          countryCode,
        //          locationCode,
        //          branchCode,
        //          institutionCode + countryCode + locationCode + branchCode)
        //{ }

        private Bic(string institutionCode, string countryCode, string locationCode, string branchCode, string value)
        {
            Contract.Requires(institutionCode != null);
            Contract.Requires(countryCode != null);
            Contract.Requires(locationCode != null);
            Contract.Requires(branchCode != null);
            Contract.Requires(value != null);

            InstitutionCode = institutionCode;
            CountryCode = countryCode;
            LocationCode = locationCode;
            BranchCode = branchCode;
            _value = value;
        }

        /// <summary>
        /// Gets the branch code.
        /// </summary>
        public string BranchCode { get; private set; }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode { get; private set; }

        /// <summary>
        /// Gets the institution code or bank code.
        /// </summary>
        public string InstitutionCode { get; private set; }

        public bool IsPrimaryOfficeBic
            => BranchCode == PrimaryOfficeBranchCode || BranchCode.Length == 0;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        public string LocationCode { get; private set; }

        public static Bic Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Bic? iban = ParseCore(value, true /* throwOnError */);

            Contract.Assume(iban.HasValue);

            return iban.Value;
        }

        public static Bic? TryParse(string value)
        {
            if (value == null)
            {
                return null;
            }

            return ParseCore(value, false /* throwOnError */);
        }

        public override string ToString() => _value;

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(BranchCode != null);
            Contract.Invariant(CountryCode != null);
            Contract.Invariant(InstitutionCode != null);
            Contract.Invariant(LocationCode != null);
            Contract.Invariant(_value != null);
        }

#endif

        private static Bic? ParseCore(string value, bool throwOnError)
        {
            Contract.Requires(value != null);

            // NB: We only perform basic validation on the input string.
            if (value.Length != 8 && value.Length != 11)
            {
                if (throwOnError)
                {
                    throw new ArgumentException(
                        "The BIC string MUST be 8 or 11 characters long.",
                        nameof(value));
                }
                else
                {
                    return null;
                }
            }

            // The first four letters define the institution or bank code.
            string institutionCode = value.Substring(0, 4);
            for (int i = 0; i < institutionCode.Length; i++)
            {
                if (!IsUpperLetter(value[i]))
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException(
                            "The first 4 characters of a BIC string MUST be made up of ASCII uppercase letters.",
                            nameof(value));
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(4, 2);

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(6, 2);

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == 11 ? value.Substring(8, 3) : String.Empty;

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }
    }

    // Implements the IEquatable<Bic> interface.
    public partial struct Bic
    {
        public static bool operator ==(Bic left, Bic right) => left.Equals(right);

        public static bool operator !=(Bic left, Bic right) => !left.Equals(right);

        public bool Equals(Bic other) => _value == other._value;

        public override bool Equals(object obj)
        {
            if (!(obj is Bic))
            {
                return false;
            }

            return Equals((Bic)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}
