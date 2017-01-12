// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    /// <summary>
    /// Represents a currency such as Euro or US Dollar.
    /// </summary>
    /// <remarks>
    /// <para>Recognized currencies are defined in ISO 4217.</para>
    /// <para>Only decimal currencies are supported.</para>
    /// <para>There's never more than one <see cref="Currency"/> instance for any given currency.
    /// You can not directly construct a currency. You must instead use one of the
    /// static factories, eg <see cref="Currency.Of"/>.</para>
    /// <para>This class does not offer extended information about the currency.</para>
    /// </remarks>
    public partial struct Currency : IEquatable<Currency>
    {
        private const int MAX_DECIMAL_PLACES = 28;
        private const int UNKNOWN_MINOR_UNITS = MAX_DECIMAL_PLACES;

        private static Dictionary<string, short?> s_UserCodes = new Dictionary<string, short?>();

        // This list is automatically generated using data obtained from the SNV website.
        private static Dictionary<string, short?> s_Codes;

        // This set is automatically generated using data obtained from the SNV website.
        private static HashSet<string> s_WithdrawnCodes;

        private readonly string _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency" /> class for the specified code.
        /// </summary>
        /// <param name="code">A string that contains the three-letter identifier defined in ISO 4217.</param>
        /// <param name="minorUnits">The value of a unit expressed in minor units. If there are no
        /// minor units, use null instead.</param>
        internal Currency(string code, short? minorUnits)
        {
            Sentinel.Demand.CurrencyCode(code);
            Demand.True(!minorUnits.HasValue || minorUnits >= 0);

            _code = code;
            MinorUnits = minorUnits;
        }

        // The list is automatically generated using data obtained from the SNV website.
        private static decimal[] Epsilons => s_Epsilons;

        // The list is automatically generated using data obtained from the SNV website.
        private static uint[] PowersOfTen => s_PowersOfTen;

        /// <summary>
        /// Gets the alphabetic code of the currency.
        /// </summary>
        /// <value>The alphabetic code of the currency.</value>
        public string Code { get { Warrant.NotNull<string>(); return _code; } }

        /// <summary>
        /// Gets the number of minor units.
        /// </summary>
        /// <value>The number of minor units; null if none defined; 28 if this number is unknown.</value>
        public short? MinorUnits { get; }

        // If the currency has no minor units (null), which only happens for meta-currencies
        // but not for all of them, we use 0.
        // If the currency has no known minor units, which is the case for all
        // legacy currencies, we use MAX_DECIMAL_PLACES as a replacement, ie an amount
        // in these currencies can take any value in the decimal range.
        // To simply things, for legacy currencies, we directly set MinorUnits to MAX_DECIMAL_PLACES,
        // but if, in the future, we change that we should also change the line below by:
        // > public int DecimalPlaces => MinorUnits.HasValue
        // >     ? (MinorUnits.Value == UNKNOWN_MINOR_UNITS ? MAX_DECIMAL_PLACES : MinorUnits.Value)
        // >     : 0;
        public int DecimalPlaces => MinorUnits ?? 0;

        public bool HasFixedDecimalPlaces => DecimalPlaces != MAX_DECIMAL_PLACES;

        /// <summary>
        /// Gets a value indicating whether the currency admits a minor currency unit.
        /// </summary>
        /// <remarks>We consider that all legacy currencies do not have a minor unit.
        /// This is actually false, but we do not have enough informations to give
        /// a sensible answer.</remarks>
        public bool HasMinorCurrency
            => MinorUnits.HasValue
            && MinorUnits.Value != 0
            && MinorUnits.Value != UNKNOWN_MINOR_UNITS;

        /// <summary>
        /// Gets a value indicating whether the currency is a meta-currency.
        /// </summary>
        /// <remarks>
        /// <para>Meta-currencies include supranational currencies (but notice that EUR
        /// is not part of them...), precious metals, the test currency, the "no"
        /// currency and currencies used in international finance.</para>
        /// <para>Meta-currencies are not attached to a specific country.
        /// Their numeric codes are in the range 900-999 and their codes are in the
        /// range XA(A)-XZ(Z). They fall in the ranges of user-assigned codes
        /// as defined by the ISO 3166 standard, ie they will never clash with
        /// those of a real country.</para>
        /// </remarks>
        /// <value>true if the currency is a meta-currency; otherwise false.</value>
        public bool IsMeta => CurrencyMetadata.IsMetaCurrency(Code);

        /// <summary>
        /// Gets the smallest positive (non zero) unit.
        /// </summary>
        /// <remarks>Returns 1m if the currency has no minor currency unit.</remarks>
        public decimal Epsilon => Epsilons[DecimalPlaces % MAX_DECIMAL_PLACES];

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "[Intentionally] When (if?) we add currencies not using a decimal system, this value will no longer look like a constant.")]
        public decimal One => 1m;

        /// <summary>
        /// Gets the minor currency unit.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the currency has no minor
        /// currency unit.</exception>
        public FractionalCurrency MinorCurrency
        {
            get
            {
                if (!HasMinorCurrency)
                {
                    throw new InvalidOperationException("XXX");
                }

                return new FractionalCurrency(this, Epsilon, MinorCurrencyCode);
            }
        }

        /// <remarks>Returns 1 if the currency has no minor currency unit.</remarks>
        private uint Factor => PowersOfTen[DecimalPlaces % MAX_DECIMAL_PLACES];

        // If the currency admits a minor currency unit, we obtain its code by "lowercasing"
        // the last character of its code: "EUR" -> "EUr". This convention is not officialy sanctioned.
        private string MinorCurrencyCode
        {
            get
            {
                Demand.True(HasMinorCurrency);
                return Code[0] + Code[1] + (Code[3] | 0x20).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified alphabetic code.
        /// </summary>
        /// <param name="code">The three letter ISO-4217 code of the currency.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is null.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        public static Currency Of(string code)
        {
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);

            short? minorUnits;
            if (!Codes.TryGetValue(code, out minorUnits))
            {
                throw new CurrencyNotFoundException(Format.Current(Strings.Currency_UnknownCode, code));
            }

            return new Currency(code, minorUnits);
        }

        public static Currency? OfOrNull(string code)
        {
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);

            short? minorUnits;
            if (!Codes.TryGetValue(code, out minorUnits)) { return null; }

            return new Currency(code, minorUnits);
        }

        public static Currency Of(string code, CurrencyTypes types)
        {
            Sentinel.Expect.CurrencyCode(code);

            var cy = OfOrNull(code, types);
            if (!cy.HasValue)
            {
                throw new CurrencyNotFoundException(Format.Current(Strings.Currency_UnknownCode, code));
            }

            return cy.Value;
        }

        public static Currency? OfOrNull(string code, CurrencyTypes types)
        {
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);

            if (types.Contains(CurrencyTypes.Active))
            {
                short? minorUnits;
                if (Codes.TryGetValue(code, out minorUnits))
                {
                    return new Currency(code, minorUnits);
                }
            }

            if (types.Contains(CurrencyTypes.Custom))
            {
                short? minorUnits;
                if (s_UserCodes.TryGetValue(code, out minorUnits))
                {
                    return new Currency(code, minorUnits);
                }
            }

            // At last, we look for a withdrawn currency.
            if (types.Contains(CurrencyTypes.Withdrawn) && WithdrawnCodes.Contains(code))
            {
                // For withdrawn currencies, ISO 4217 does not provide any information
                // concerning the minor units. See the property DecimalPlaces for more info.
                return new Currency(code, UNKNOWN_MINOR_UNITS);
            }

            return null;
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated with the specified region.
        /// </summary>
        /// <param name="regionInfo">A region info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="regionInfo"/> is null.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified region.</exception>
        /// <returns>The currency for the specified region info.</returns>
        public static Currency ForRegion(RegionInfo regionInfo)
        {
            Require.NotNull(regionInfo, nameof(regionInfo));

            //var code = regionInfo.ISOCurrencySymbol;
            //Contract.Assume(code != null);
            //Contract.Assume(code.Length != 0);   // Should not be necessary, but CCCheck insists.
            //Contract.Assume(Ascii.IsUpperLetter(code));
            //Contract.Assume(code.Length == 3);

            return Of(regionInfo.ISOCurrencySymbol);
        }

        public static Currency ForCurrentRegion() => ForRegion(RegionInfo.CurrentRegion);

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated
        /// with the specified culture.
        /// </summary>
        /// <param name="cultureInfo">A culture info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cultureInfo"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureInfo"/> is neutral.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the specified culture.</exception>
        /// <returns>The currency for the specified culture info.</returns>
        public static Currency ForCulture(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));

            if (cultureInfo.IsNeutralCulture)
            {
                throw new ArgumentException(Strings.Argument_NeutralCultureNotSupported, nameof(cultureInfo));
            }

            return ForRegion(new RegionInfo(cultureInfo.Name));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the culture
        /// used by the current thread.
        /// </summary>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the current culture.</exception>
        /// <returns>The currency for the culture used by the current thread.</returns>
        public static Currency ForCurrentCulture() => ForCulture(CultureInfo.CurrentCulture);

        // This method allows to register currencies that are not part of ISO 4217.
        // For details, see https://en.wikipedia.org/wiki/ISO_4217.
        // If you have more than one currency to register, you should use RegisterCurrencies()
        // instead - it will be more efficient.
        // FIXME: This method is not thread-safe.
        public static bool RegisterCurrency(string code, short? minorUnits)
        {
            Sentinel.Require.CurrencyCode(code, nameof(code));
            Demand.True(!minorUnits.HasValue || minorUnits >= 0);

            if (s_UserCodes.ContainsKey(code)
                || Codes.ContainsKey(code)
                || WithdrawnCodes.Contains(code)) { return false; }

            // We work on a temporary copy of s_UserCodes. This is not very efficient but ensures
            // that s_UserCodes does not end up in a broken state if anything bad happens.
            // Also note that, with competing threads, we may end up creating 'tmpCopy' more
            // than once. Anyway, we do not expect this method to be called very often, if ever.
            var tmpCopy = s_UserCodes.ToDictionary(_ => _.Key, _ => _.Value);
            tmpCopy[code] = minorUnits;

            // We use a volatile write to make sure that instructions don't get re-ordered.
            Volatile.Write(ref s_UserCodes, tmpCopy);

            return true;
        }

        // See RegisterCurrency() for explanations.
        // FIXME: This method is not thread-safe.
        public static bool RegisterCurrencies(Dictionary<string, short?> currencies)
        {
            Require.NotNull(currencies, nameof(currencies));

            foreach (var pair in currencies)
            {
                string code = pair.Key;
                if (code == null || code.Length != 3 || !Ascii.IsUpperLetter(code))
                {
                    return false;
                }

                short? minorUnits = pair.Value;
                if (minorUnits.HasValue && minorUnits < 0)
                {
                    return false;
                }

                if (s_UserCodes.ContainsKey(code)
                    || Codes.ContainsKey(code)
                    || WithdrawnCodes.Contains(code))
                {
                    return false;
                }
            }

            var tmpCopy = s_UserCodes.ToDictionary(_ => _.Key, _ => _.Value);
            foreach (var pair in currencies)
            {
                tmpCopy[pair.Key] = pair.Value;
            }

            Volatile.Write(ref s_UserCodes, tmpCopy);

            return true;
        }

        public bool IsNativeTo(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));

            if (cultureInfo.IsNeutralCulture) { return false; }

            var ri = new RegionInfo(cultureInfo.Name);

            return ri.ISOCurrencySymbol == Code;
        }

        /// <summary>
        /// Converts an amount from minor units to major units.
        /// </summary>
        /// <param name="minor">The amount in minor units to convert.</param>
        internal decimal ConvertToMajor(decimal minor) => Epsilon * minor;

        /// <summary>
        /// Converts an amount from major units to minor units.
        /// </summary>
        /// <param name="major">The amount in major units to convert.</param>
        internal decimal ConvertToMinor(decimal major) => Factor * major;

        /// <summary>
        /// Returns a string containing the code of the currency.
        /// </summary>
        /// <returns>A string containing the code of the currency.</returns>
        public override string ToString()
        {
            Warrant.NotNull<string>();

            return Code;
        }
    }

    // Interface IEquatable<Currency>.
    public partial struct Currency
    {
        public static bool operator ==(Currency left, Currency right) => left.Equals(right);

        public static bool operator !=(Currency left, Currency right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Currency other) => Code == other.Code;

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            if (!(obj is Currency)) { return false; }

            return Equals((Currency)obj);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        // TODO: Since there are so few currencies, we could cache the hash code?
        public override int GetHashCode() => Code.GetHashCode();
    }

    // Aliases for some of the most commonly used currencies.
    public partial struct Currency
    {
        /// <summary>
        /// Gets the currency unit for the pseudo-currency for transactions where no currency is involved.
        /// </summary>
        public static Currency None => Of("XXX");

        /// <summary>
        /// Gets the currency unit for the currency specifically reserved for testing purposes.
        /// </summary>
        public static Currency Test => Of("XTS");

        /// <summary>
        /// Gets the currency unit for the "Euro".
        /// </summary>
        public static Currency Euro => Of("EUR");

        /// <summary>
        /// Gets the currency unit for the "(British) "Pound Sterling".
        /// </summary>
        public static Currency PoundSterling => Of("GBP");

        /// <summary>
        /// Gets the currency unit for the "Swiss Franc".
        /// </summary>
        public static Currency SwissFranc => Of("CHF");

        /// <summary>
        /// Gets the currency unit for the "United States Dollar".
        /// </summary>
        public static Currency UnitedStatesDollar => Of("USD");

        /// <summary>
        /// Gets the currency unit for the "Japanese Yen".
        /// </summary>
        public static Currency Yen => Of("JPY");

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for gold.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AU.</remarks>
        public static Currency Gold => Of("XAU");

        /// <summary>
        /// Gets the currency unit class for the pseudo-currency for palladium.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PD.</remarks>
        public static Currency Palladium => Of("XPD");

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for platinum.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PT.</remarks>
        public static Currency Platinum => Of("XPT");

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for silver.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AG.</remarks>
        public static Currency Silver => Of("XAG");
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public partial struct Currency
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_code != null);
            Contract.Invariant(_code.Length == 3);
        }
    }
}

#endif
