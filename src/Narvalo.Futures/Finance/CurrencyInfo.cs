// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public sealed class CurrencyInfo
    {
        private const char META_CURRENCY_MARK = 'X';

        private readonly string _code;
        private readonly short _numericCode;

        public CurrencyInfo(string code, short numericCode)
        {
            Require.NotNullOrEmpty(code, nameof(code));
            //ContractFor.CurrencyCode(code);
            Contract.Requires(
                numericCode >= 0 && numericCode < 1000,
                "The numeric code MUST be strictly greater than 0 and less than 1000.");

            _code = code;
            _numericCode = numericCode;
        }

        public string Code
        {
            get { Warrant.NotNull<string>(); return _code; }
        }

        /// <summary>
        /// Gets the currency unit of the currency.
        /// </summary>
        /// <value>The currency unit of the currency.</value>
        public Currency Currency=> Currency.Of(Code);

        public string EnglishName { get; set; }

        public string EnglishRegionName { get; set; }

        public bool HasNumericCode => NumericCode != 0;

        public bool IsFund { get; set; }

        public bool IsMetaCurrency => Code[0] == META_CURRENCY_MARK;

        public short? MinorUnits { get; set; }

        public short NumericCode => _numericCode;

        public bool Superseded { get; set; }

        public bool Withdrawn => !Superseded;

        /// <summary>
        /// Obtains the list of circulating currencies.
        /// </summary>
        /// <example>
        /// Obtains the list of currency/country using the euro:
        /// <code>
        /// CurrencyInfo.GetCurrencies().Where(_ => _.Code == "EUR");
        /// </code>
        /// </example>
        public static CurrencyInfoCollection GetCurrencies()
            => GetCurrencies(CurrencyTypes.Circulating);

        /// <summary>
        /// Obtains the list of supported currencies filtered by the specified
        /// <see cref="CurrencyTypes"/> parameter.
        /// </summary>
        /// <param name="types">A bitwise combination of the enumeration values
        /// that filter the currencies to retrieve.</param>
        /// <returns>An enumeration that contains the currencies specified by
        /// the <paramref name="types"/> parameter.</returns>
        public static CurrencyInfoCollection GetCurrencies(CurrencyTypes types)
            => CurrencyInfoProvider.Current.GetCurrencies(types);

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return EnglishName + "(" + EnglishRegionName + ")";
        }
    }
}
