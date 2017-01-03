// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    /// <summary>
    /// Represents a currency such as Euro or US Dollar.
    /// </summary>
    /// <remarks>
    /// <para>Recognized currencies are defined in ISO 4217.</para>
    /// <para>There's never more than one <see cref="Currency"/> instance for any given currency.
    /// You can not directly construct a currency. You must instead use the
    /// static factories: <see cref="Currency.Of"/>.</para>
    /// <para>This class follows value type semantics when it comes to equality.</para>
    /// <para>This class does not offer extended information about the currency.</para>
    /// </remarks>
    public partial struct Currency : IEquatable<Currency>
    {
        private const char META_CURRENCY_MARK = 'X';

        // The list is automatically generated using data obtained from the SNV website.
        // The volatile keyword is only for correctness.
        // REVIEW: Instead of using a volatile field, wouldn't it be better to create a temporary
        // dictionary then swap the references?
        private volatile static Dictionary<string, short?> s_Codes;

        private static readonly decimal[] s_Units = new decimal[4]
        {
            1m,
            0.1m,
            0.01m,
            0.001m,
        };

        private readonly string _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency" /> class for the specified code.
        /// </summary>
        /// <param name="code">A string that contains the three-letter identifier defined in ISO 4217.</param>
        internal Currency(string code, short? minorUnits)
        {
            Sentinel.Demand.CurrencyCode(code);

            _code = code;
            MinorUnits = minorUnits;
        }

        /// <summary>
        /// Gets the alphabetic code of the currency.
        /// </summary>
        /// <value>The alphabetic code of the currency.</value>
        public string Code { get { Warrant.NotNull<string>(); return _code; } }

        /// <summary>
        /// Gets the number of minor units.
        /// </summary>
        /// <value>The number of minor units; <see langword="null"/> if none defined.</value>
        public short? MinorUnits { get; }

        public short DecimalPlaces => MinorUnits ?? 0;

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
        /// <value><see langword="true"/> if the currency is a meta-currency; otherwise <see langword="false"/>.</value>
        public bool IsMeta => IsMetaCurrency(Code);

        // Gets the smallest non zero unit.
        // Returns 1m if the currency has no minor unit.
        public decimal SmallestUnit => s_Units[DecimalPlaces];

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified alphabetic code.
        /// </summary>
        /// <param name="code">The three letter ISO-4217 code of the currency.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        public static Currency Of(string code)
        {
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);

            Contract.Assume(Codes != null);
            short? minorUnits;
            if (!Codes.TryGetValue(code, out minorUnits))
            {
                throw new CurrencyNotFoundException(Format.Current(Strings.Currency_UnknownCode, code));
            }

            return new Currency(code, minorUnits);
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated with the specified region.
        /// </summary>
        /// <param name="regionInfo">A region info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="regionInfo"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified region.</exception>
        /// <returns>The currency for the specified region info.</returns>
        public static Currency OfRegion(RegionInfo regionInfo)
        {
            Require.NotNull(regionInfo, nameof(regionInfo));

            //var code = regionInfo.ISOCurrencySymbol;
            //Contract.Assume(code != null);
            //Contract.Assume(code.Length != 0);   // Should not be necessary, but CCCheck insists.
            //Contract.Assume(Ascii.IsUpperLetter(code));
            //Contract.Assume(code.Length == 3);

            return Of(regionInfo.ISOCurrencySymbol);
        }

        public static Currency OfCurrentRegion() => OfRegion(RegionInfo.CurrentRegion);

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated
        /// with the specified culture.
        /// </summary>
        /// <param name="cultureInfo">A culture info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cultureInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureInfo"/> is neutral.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the specified culture.</exception>
        /// <returns>The currency for the specified culture info.</returns>
        public static Currency OfCulture(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));

            if (cultureInfo.IsNeutralCulture)
            {
                throw new ArgumentException(Strings.Argument_NeutralCultureNotSupported, nameof(cultureInfo));
            }

            return OfRegion(new RegionInfo(cultureInfo.Name));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the culture
        /// used by the current thread.
        /// </summary>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the current culture.</exception>
        /// <returns>The currency for the culture used by the current thread.</returns>
        public static Currency OfCurrentCulture() => OfCulture(CultureInfo.CurrentCulture);

        // This method allows to register currencies that are not part of ISO 4217.
        // For details, see https://en.wikipedia.org/wiki/ISO_4217.
        // FIXME: Concurrent access.
        public static bool RegisterCurrency(string code, short? minorUnits)
        {
            Sentinel.Require.CurrencyCode(code, nameof(code));

            Contract.Assume(Codes != null);
            if (Codes.ContainsKey(code)) { return false; }

            // We work on a temporary copy of Codes. This is not very efficient but ensures
            // that s_Codes does not end up in a broken state if anything bad happens.
            // Anyway, we do not expect this method to be called very often, if ever.
            var tmpCopy = Codes.ToDictionary(_ => _.Key, _ => _.Value);
            tmpCopy[code] = minorUnits;

            s_Codes = tmpCopy;

            return true;
        }

        // TODO: What about EUR, CFP... for this, is it enough to check the country code too
        // (the first two letters)?
        internal static bool IsMetaCurrency(string currencyCode)
        {
            Demand.NotNullOrEmpty(currencyCode);

            return currencyCode[0] == META_CURRENCY_MARK;
        }

        public bool IsNativeTo(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));

            if (cultureInfo.IsNeutralCulture) { return false; }

            var ri = new RegionInfo(cultureInfo.Name);

            return ri.ISOCurrencySymbol == Code;
        }

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
