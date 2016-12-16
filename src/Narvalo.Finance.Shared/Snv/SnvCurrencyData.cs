// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Globalization;
    using Narvalo.Finance.Internal;

    /// <summary>
    /// Provides information about a localized currency.
    /// </summary>
    /// <remarks>
    /// Different currencies may have the same <see cref="SnvCurrencyData.Code"/>
    /// and <see cref="SnvCurrencyData.NumericCode"/> but be associated to different
    /// regions/countries. There is no 1-1 correspondance between currencies
    /// and currency infos.
    /// </remarks>
    public sealed partial class SnvCurrencyData
    {
        private const char META_CURRENCY_MARK = 'X';

        public const short FallbackNumericCode = 0;

        private readonly string _code;
        private readonly short _numericCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnvCurrencyData" /> class
        /// for the specified alphabetic and numeric codes.
        /// </summary>
        /// <param name="code">A string that contains a three-letter code defined in ISO 4217.</param>
        /// <param name="numericCode">A numeric identifier defined in ISO 4217.</param>
        /// <param name="superseded"></param>
        private SnvCurrencyData(string code, short numericCode, bool superseded)
        {
            Sentinel.Require.CurrencyCode(code, nameof(code));
            Demand.Range(numericCode >= 0 && numericCode < 1000);

            _code = code;
            _numericCode = numericCode;
            Superseded = superseded;
        }

        public static SnvCurrencyData Create(string code, short numericCode)
            => CreateCore(code, numericCode, false);

        public static SnvCurrencyData CreateLegacy(string code, short? numericCode)
            => numericCode.HasValue
            ? CreateCore(code, numericCode.Value, true)
            : new SnvCurrencyData(code, FallbackNumericCode, true);

        private static SnvCurrencyData CreateCore(string code, short numericCode, bool superseded)
        {
            Require.Range(numericCode > 0 && numericCode < 1000, nameof(numericCode),
                "The numeric code MUST be greater than 0 and strictly less than 1000.");

            return new SnvCurrencyData(code, numericCode, superseded);
        }

        /// <summary>
        /// Gets the alphabetic code of the currency.
        /// </summary>
        /// <remarks>
        /// A currency is uniquely identified by a three letter code, based on ISO 4217.
        /// Valid currency codes are three upper-case ASCII letters.
        /// If the currency is not a meta-currency, the first two letters usually match
        /// an alpha-2 country code as found in the ISO 3166 (Ecuador, Haiti, El Savador...
        /// are counter-examples).
        /// The third letter is usually the initial of the currency name.
        /// For instance: USD = US (United States) + D (Dollar).
        /// </remarks>
        /// <value>The alphabetic code of the currency.</value>
        public string Code
        {
            get { Warrant.NotNull<string>(); return _code; }
        }

        /// <summary>
        /// Gets the numeric code of the currency.
        /// </summary>
        /// <remarks>
        /// <para>The numeric code is an alternative to the alphabetic code.</para>
        /// <para>It usually matches the numeric code of the country as defined by the ISO 3166,
        /// but this is not always true. Obviously, supranational currencies, like the EUR, are
        /// currencies where we can not infer the country from this code.
        /// I think, but I have not verified, that for codes strictly lower than 900 we do actually
        /// get an ISO 3166 country code. Funny enough, Afghanistan or Angola are among those
        /// actual countries for which the currency has a numeric code that has nothing to do
        /// with their country code.
        /// For instance: 840 = United States.</para>
        /// </remarks>
        /// <seealso cref="HasNumericCode"/>
        /// <value>The numeric code of the currency; zero if the currency has no numeric code.</value>
        public short NumericCode => _numericCode;

        /// <summary>
        /// Gets or sets a value indicating whether the currency is no longer in use.
        /// </summary>
        /// <value><see langword="true"/> if the currency is no longer in use; otherwise <see langword="false"/>.
        /// The default is <see langword="false"/>.</value>
        public bool Superseded { get; }

        public bool Withdrawn => !Superseded;

        /// <summary>
        /// Gets or sets the full name of the currency in English.
        /// </summary>
        /// <remarks>
        /// This name is not guaranteed to match the value of RegionInfo.CurrencyEnglishName.
        /// </remarks>
        /// <value>The full name of the currency in English.</value>
        public string EnglishName { get; internal set; }

        /// <summary>
        /// Gets or sets the full name of the country/region in English.
        /// </summary>
        /// <remarks>
        /// <para>This name is not guaranteed to match the value of
        /// <see cref="RegionInfo.EnglishName"/>.</para>
        /// <para>Most meta-currencies do not belong to a region but they still
        /// get a pseudo region name. Besides that, most of these currencies use
        /// a region name that starts with "ZZ" to make it clear.</para>
        /// </remarks>
        /// <value>The full name of the country/region in English.</value>
        public string EnglishCountryName { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the currency represents a fund.
        /// </summary>
        /// <value><see langword="true"/> if the currency represents a fund; otherwise <see langword="false"/>.
        /// The default is <see langword="false"/>.</value>
        public bool IsFund { get; internal set; }

        /// <summary>
        /// Gets or sets the number of minor units.
        /// </summary>
        /// <value>The number of minor units; <see langword="null"/> if none defined.
        /// The default is <see langword="null"/>.</value>
        public short? MinorUnits { get; internal set; }

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
        public bool IsMetaCurrency => Code[0] == META_CURRENCY_MARK;

        /// <summary>
        /// Gets a value indicating whether the currency has a numeric code.
        /// </summary>
        /// <value><see langword="true"/> if the currency has a numeric code; otherwise <see langword="false"/>.</value>
        public bool HasNumericCode => NumericCode != 0;

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return EnglishName + " (" + Code + ")";
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance.Snv
{
    using System.Diagnostics.Contracts;

    public sealed partial class SnvCurrencyData
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_code != null);
            Contract.Invariant(_code.Length == 3);
            Contract.Invariant(_numericCode >= 0 && _numericCode < 1000);
        }
    }
}

#endif