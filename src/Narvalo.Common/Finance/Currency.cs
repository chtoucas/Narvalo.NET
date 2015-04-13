// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Narvalo.Internal;

    /// <summary>
    /// Represents a currency unit such as Euro or US Dollar.
    /// </summary>
    /// <remarks>
    /// <para>Recognized currencies are defined in ISO 4217.</para>
    /// <para>There's never more than one <see cref="Currency"/> instance for any given currency.
    /// Therefore, you can not directly construct a currency. You must instead use one of the
    /// static factories: <see cref="Currency.Of"/>, <see cref="Currency.OfCulture"/> 
    /// or <see cref="Currency.OfCurrentCulture"/>.</para>
    /// <para>This class follows value type semantics when it comes to equality.</para>
    /// <para>This class does not offer extended information about the currency.
    /// If you needed so, you may use to the <see cref="CurrencyInfo"/> class.</para>
    /// </remarks>
    [Serializable]
    public sealed partial class Currency : IEquatable<Currency>
    {
        private const char META_CURRENCY_MARK = 'X';

        // We cache all requested currencies.
        private static readonly ConcurrentDictionary<string, Currency> s_Cache
             = new ConcurrentDictionary<string, Currency>();

        private readonly string _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency" /> class for the specified code.
        /// </summary>
        /// <param name="code">A string that contains the three-letter identifier defined in ISO 4217.</param>
        internal Currency(string code)
        {
            Contract.Requires(code != null);
            Contract.Requires(code.Length == 3);

            _code = code;
        }

        /// <summary>
        /// Gets the alphabetic code of the currency.
        /// </summary>
        /// <value>The alphabetic code of the currency.</value>
        public string Code
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return _code;
            }
        }

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
        public bool IsMetaCurrency
        {
            get { return Code[0] == META_CURRENCY_MARK; }
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified alphabetic code.
        /// </summary>
        /// <param name="code">The three letter ISO-4217 code of the currency.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        public static Currency Of(string code)
        {
            Require.NotNullOrWhiteSpace(code, "code");
            ContractFor.CurrencyCode(code);
            Contract.Ensures(Contract.Result<Currency>() != null);

            // Fast-track for the most commonly used currencies.
            switch (code)
            {
                case Euro.Code:
                    return Euro.Currency;
                case PoundSterling.Code:
                    return PoundSterling.Currency;
                case SwissFranc.Code:
                    return SwissFranc.Currency;
                case UnitedStatesDollar.Code:
                    return UnitedStatesDollar.Currency;
                case Yen.Code:
                    return Yen.Currency;
                case NoCurrency.Code:
                    return NoCurrency.Currency;
            }

            return s_Cache.GetOrAdd(code, GetCurrency_).AssumeNotNull();
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated 
        /// with the specified culture.
        /// </summary>
        /// <param name="cultureInfo">A culture info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cultureInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the specified culture.</exception>
        /// <returns>The currency for the specified culture info.</returns>
        public static Currency OfCulture(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, "cultureInfo");
            Contract.Ensures(Contract.Result<Currency>() != null);

            return OfRegion(new RegionInfo(cultureInfo.LCID));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the culture
        /// used by the current thread.
        /// </summary>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the current culture.</exception>
        /// <returns>The currency for the culture used by the current thread.</returns>
        public static Currency OfCurrentCulture()
        {
            Contract.Ensures(Contract.Result<Currency>() != null);

            return OfRegion(new RegionInfo(CultureInfo.CurrentCulture.LCID));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated with the specified region.
        /// </summary>
        /// <param name="regionInfo">A region info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="regionInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the specified region.</exception>
        /// <returns>The currency for the specified region info.</returns>
        public static Currency OfRegion(RegionInfo regionInfo)
        {
            Require.NotNull(regionInfo, "regionInfo");
            Contract.Ensures(Contract.Result<Currency>() != null);

            var code = regionInfo.ISOCurrencySymbol.AssumeNotNull();

            Contract.Assume(code.Length == 3);

            return Of(code);
        }

        /// <summary>
        /// Returns a string containing the code of the currency.
        /// </summary>
        /// <returns>A string containing the code of the currency.</returns>
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return _code;
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified alphabetic code.
        /// </summary>
        /// <remarks>
        /// Contrary to the <see cref="Currency.Of"/> method, this method always return a fresh object.
        /// </remarks>
        /// <param name="code">The three letter ISO-4217 code of the currency.</param>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        private static Currency GetCurrency_(string code)
        {
            Contract.Requires(code != null);
            Contract.Requires(code.Length == 3);
            Contract.Ensures(Contract.Result<Currency>() != null);

            if (!CurrencyProvider.Current.CurrencyCodes.Contains(code))
            {
                throw new CurrencyNotFoundException("Unknown currency: " + code + ".");
            }

            return new Currency(code);
        }
    }

    /// <content>
    /// Implements the <see cref="IEquatable{Currency}"/> interface.
    /// </content>
    public partial class Currency
    {
        public static bool operator ==(Currency left, Currency right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                // If left is null, returns true if right is null too, false otherwise.
                return Object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Currency left, Currency right)
        {
            return !(left == right);
        }

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Currency other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                // Remember, "this" is never null in C#.
                return false;
            }

            return this._code == other._code;
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        [SuppressMessage("Gendarme.Rules.Portability", "MonoCompatibilityReviewRule",
            Justification = "[Intentionally] Method marked as MonoTODO.")]
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                // Remember, "this" is never null in C#.
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                // "obj" and "this" are exactly the same object.
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                // "obj" and "this" are not of the same type.
                return false;
            }

            return Equals(obj as Currency);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode()
        {
            return _code.GetHashCode();
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_code != null);
            Contract.Invariant(_code.Length == 3);
        }

#endif
    }
}
