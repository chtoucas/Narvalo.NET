// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Narvalo.Internal;

    /// <summary>
    /// Provides information about a specific combination of currency and region/country.
    /// </summary>
    /// <remarks>
    /// Different currencies may have the same <see cref="CurrencyInfo.Code"/>
    /// and <see cref="CurrencyInfo.NumericCode"/> but be associated to different 
    /// regions/countries. There is NOT a 1-1 correspondance between currencies
    /// and currency infos.
    /// </remarks>
    [Serializable]
    public sealed class CurrencyInfo
    {
        const char MetaCurrencyMark_ = 'X';

        readonly string _code;
        readonly short _numericCode;
        readonly Lazy<RegionInfo> _regionInfo;

        string _symbol;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyInfo" /> class 
        /// for the specified alphabetic and numeric codes.
        /// </summary>
        /// <param name="code">A string that contains a three-letter code defined in ISO 4217.</param>
        /// <param name="numericCode">A numeric identifier defined in ISO 4217.</param>
        internal CurrencyInfo(string code, short numericCode)
        {
            Enforce.NotNull(code, "code");
            Contract.Requires(
                code.Length == 3 && code.All(c => { var pos = (int)c; return pos >= 65 && pos <= 90; }),
                "The code MUST be composed of exactly 3 letters, all CAPS and ASCII.");
            Contract.Requires(
                numericCode > 0 && numericCode < 1000,
                "The numeric code MUST be strictly greater than 0 and less than 1000.");

            _code = code;
            _numericCode = numericCode;

            _regionInfo = new Lazy<RegionInfo>(FindRegionInfo_);
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
        public string Code { get { return _code; } }

        /// <summary>
        /// Gets the full name of the currency in English.
        /// </summary>
        /// <remarks>
        /// This name is not guaranteed to match the value of 
        /// <see cref="System.Globalization.RegionInfo.CurrencyEnglishName"/>
        /// from the <see cref="RegionInfo"/> property.
        /// </remarks>
        /// <value>The full name of the currency in English.</value>
        public string EnglishName { get; internal set; }

        /// <summary>
        /// Gets the full name of the country/region in English.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This name has nothing to do with the value of 
        /// <see cref="System.Globalization.RegionInfo.EnglishName"/>
        /// from the <see cref="RegionInfo"/> property.
        /// </para>
        /// <para>
        /// Most meta-currencies do not belong to a region but they still
        /// get a pseudo region name. Besides that, most of these currencies use
        /// a region name that starts with "ZZ" to make it clear.
        /// </para>
        /// </remarks>
        /// <value>The full name of the country/region in English.</value>
        public string EnglishRegionName { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the currency is no longer in use.
        /// </summary>
        /// <value><c>true</c> if the currency is no longer in use; otherwise <c>false</c>.</value>
        public bool IsDiscontinued { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the currency represents a fund.
        /// </summary>
        /// <value><c>true</c> if the currency represents a fund; otherwise <c>false</c>.</value>
        public bool IsFund { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the currency is a meta-currency.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Meta-currencies include supranational currencies (but notice that EUR 
        /// is not part of them...), precious metals, the test currency, the "no" 
        /// currency and currencies used in international finance.
        /// </para>
        /// <para>
        /// Meta-currencies are not attached to a specific country.
        /// Their numeric codes are in the range 900-999 and their codes are in the
        /// range XA(A)-XZ(Z). They fall in the ranges of user-assigned codes 
        /// as defined by the ISO 3166 standard, ie they will never clash with 
        /// those of a real country.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> if the currency is a meta-currency; otherwise <c>false</c>.</value>
        public bool IsMetaCurrency
        {
            get { return Code[0] == MetaCurrencyMark_; }
        }

        /// <summary>
        /// Gets the number of minor units.
        /// </summary>
        /// <value>The number of minor units; <c>null</c> if none defined</value>
        public short? MinorUnits { get; internal set; }

        /// <summary>
        /// Gets the numeric code of the currency.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The numeric code is an alternative to the alphabetic code.
        /// </para>
        /// <para>
        /// It usually matches the numeric code of the country as defined by the ISO 3166,
        /// but this is not always true. Obviously, supranational currencies, like the EUR, are 
        /// currencies where we can not infer the country from this code.
        /// I think, but I have not verified, that for codes strictly lower than 900 we do actually
        /// get an ISO 3166 country code. Funny enough, Afghanistan or Angola are among those
        /// actual countries for which the currency has a numeric code that has nothing to do 
        /// with their country code.
        /// For instance: 840 = United States.
        /// </para>
        /// </remarks>
        /// <value>The numeric code of the currency.</value>
        public short NumericCode { get { return _numericCode; } }

        /// <summary>
        /// Gets the region info of the currency.
        /// </summary>
        /// <remarks>
        /// The <see cref="Code"/> property is not guaranteed to match the value of 
        /// <see cref="System.Globalization.RegionInfo.ISOCurrencySymbol"/>
        /// from the <see cref="RegionInfo"/> property. Indeed, the region info
        /// only uses the most recent currency.
        /// Moreover, a country might use more than one currency but .NET will only allow for
        /// one currency. For instance, El Salvador use both USD and SVC but .NET
        /// only knows about USD.
        /// </remarks>
        /// <value>The region info of the currency; <c>null</c> if none found.</value>
        public RegionInfo RegionInfo { get { return _regionInfo.Value; } }

        /// <summary>
        /// Gets or sets the currency symbol.
        /// </summary>
        /// <value>The currency symbol of the currency.</value>
        public string Symbol
        {
            get
            {
                if (_symbol == null) {
                    string symbol;

                    if (RegionInfo != null && RegionInfo.ISOCurrencySymbol == Code) {
                        // If the RegionInfo do refer to the currency we are currently 
                        // dealing with, the CurrencySymbol should give us the correct answer.
                        symbol = RegionInfo.CurrencySymbol;
                    }
                    else {
                        symbol = CurrencyProvider.Current.GetFallbackSymbol(Code);
                    }

                    _symbol = symbol;
                }

                return _symbol;
            }
            set { _symbol = value; }
        }

        /// <summary />
        public override string ToString()
        {
            return String.Format("{0} ({1})", EnglishName, EnglishRegionName);
        }

        /// <summary>
        /// Obtains the list of currently active currencies.
        /// </summary>
        /// <example>
        /// Obtains the list of currency/country using the euro:
        /// <code>
        /// CurrencyInfo.GetCurrencies().Where(_ => _.Code == "EUR");
        /// </code>
        /// </example>
        public static IEnumerable<CurrencyInfo> GetCurrencies()
        {
            return GetCurrencies(CurrencyTypes.CurrentCurrencies);
        }

        /// <summary>
        /// Obtains the list of supported currencies filtered by the specified
        /// <see cref="CurrencyTypes"/> parameter.
        /// </summary>
        /// <param name="types">A bitwise combination of the enumeration values that filter the 
        /// currencies to retrieve.</param>
        /// <returns>An enumeration that contains the currencies specified by the <paramref name="types"/> 
        /// parameter.</returns>
        public static IEnumerable<CurrencyInfo> GetCurrencies(CurrencyTypes types)
        {
            return CurrencyProvider.Current.GetCurrencies(types);
        }

        RegionInfo FindRegionInfo_()
        {
            // Data from the ISO 4217 offer several hints from which we can infer the country/region:
            // - If the numeric code is strictly less than 900, it SHOULD match 
            //   the numeric country code defined by ISO 3166.
            // - The first two letters from the alphabetic code SHOULD match
            //   the country alpha-2 code defined by ISO 3166.
            // - The english name of the region.
            // Using the numeric code is not good. For instance, we would miss most of the European 
            // countries which use the EUR supranational currency whose code (978) does not relate 
            // to the actual country. For exactly the same reason we can not use the alphabetic code.
            var region = Iso3166.FindRegionByEnglishName(EnglishRegionName).SingleOrDefault();

            if (NumericCode < 900) {
                if (region == null) {
                    // This should not come as a surprise, .NET does not cover the full range 
                    // of countries defined by ISO 3166.
                    Debug.WriteLine("No region found for: " + EnglishRegionName);
                }
            }

            return region;
        }
    }
}