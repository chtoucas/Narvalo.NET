// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Narvalo.Globalization;
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
        // We cache all requested currencies.
        private static readonly ConcurrentDictionary<string, Currency> s_Cache
             = new ConcurrentDictionary<string, Currency>();

        private readonly string _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency" /> class for the specified code.
        /// </summary>
        /// <param name="code">A string that contains the three-letter identifier defined in ISO 4217.</param>
        private Currency(string code)
        {
            // We do not fully validate the input here since this should be taken care of by the provider.
            Contract.Requires(code != null);

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
        /// Obtains an instance of the <see cref="Currency" /> class for the specified alphabetic code.
        /// </summary>
        /// <param name="code">The three letter ISO-4217 code of the currency.</param>
        /// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">No currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        public static Currency Of(string code)
        {
            Contract.Requires(code != null);
            Contract.Ensures(Contract.Result<Currency>() != null);

            return s_Cache.GetOrAdd(code, GetCurrency_).AssumeNotNull();
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated 
        /// with the specified culture.
        /// </summary>
        /// <param name="cultureInfo">A culture info.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cultureInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">No currency exists for the specified culture.</exception>
        /// <returns>The currency for the specified culture info.</returns>
        public static Currency OfCulture(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, "cultureInfo");
            Contract.Ensures(Contract.Result<Currency>() != null);

            return Of(new RegionInfo(cultureInfo.LCID).ISOCurrencySymbol.AssumeNotNull());
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the culture
        /// used by the current thread.
        /// </summary>
        /// <exception cref="CurrencyNotFoundException">No currency exists for the current culture.</exception>
        /// <returns>The currency for the culture used by the current thread.</returns>
        public static Currency OfCurrentCulture()
        {
            Contract.Ensures(Contract.Result<Currency>() != null);

            return OfCulture(CultureInfo.CurrentCulture);
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
        /// <exception cref="CurrencyNotFoundException">No currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        private static Currency GetCurrency_(string code)
        {
            Promise.NotNull(code);
            Contract.Ensures(Contract.Result<Currency>() != null);

            if (!CurrencyProvider.Current.CurrencyCodes.Contains(code))
            {
                throw new CurrencyNotFoundException("Unknown currency: " + code + ".");
            }

            return new Currency(code);
        }
    }

    /// <content>
    /// Provides user-friendly access to some of the most common currencies.
    /// </content>
    public partial class Currency
    {
        // We aggressively cache the most common currencies.
        private static Currency s_Dollar = Of("USD");
        private static Currency s_Euro = Of("EUR");
        private static Currency s_Pound = Of("GBP");
        private static Currency s_SwissFranc = Of("CHF");
        private static Currency s_Yen = Of("JPY");

        /// <summary>
        /// Gets the "Special Drawing Right" currency.
        /// </summary>
        /// <remarks>This is the currency used by the invariant culture.</remarks>
        /// <value>The "Special Drawing Right" currency.</value>
        public static Currency Invariant
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XDR");
            }
        }

        /// <summary>
        /// Gets the pseudo-currency for transactions where no currency is involved.
        /// </summary>
        /// <value>The pseudo-currency for transactions where no currency is involved.</value>
        public static Currency None
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XXX");
            }
        }

        /// <summary>
        /// Gets the currency specifically reserved for testing purposes.
        /// </summary>
        /// <value>The currency specifically reserved for testing purposes.</value>
        public static Currency Test
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XTS");
            }
        }

        /// <summary>
        /// Gets the (British) Pound Sterling currency.
        /// </summary>
        /// <value>The (British) Pound Sterling currency.</value>
        public static Currency Pound
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return s_Pound;
            }
        }

        /// <summary>
        /// Gets the Euro currency.
        /// </summary>
        /// <value>The Euro currency.</value>
        public static Currency Euro
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return s_Euro;
            }
        }

        /// <summary>
        /// Gets the United States Dollar currency.
        /// </summary>
        /// <value>The United States Dollar currency.</value>
        public static Currency Dollar
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return s_Dollar;
            }
        }

        /// <summary>
        /// Gets the Swiss Franc currency.
        /// </summary>
        /// <value>The Swiss Franc currency.</value>
        public static Currency SwissFranc
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return s_SwissFranc;
            }
        }

        /// <summary>
        /// Gets the Japanese Yen currency.
        /// </summary>
        /// <value>The Japanese Yen currency.</value>
        public static Currency Yen
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return s_Yen;
            }
        }

        /// <summary>
        /// Gets the pseudo-currency for gold.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AU.</remarks>
        /// <value>The pseudo-currency for gold.</value>
        public static Currency Gold
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XAU");
            }
        }

        /// <summary>
        /// Gets the pseudo-currency for palladium.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PD.</remarks>
        /// <value>The pseudo-currency for palladium.</value>
        public static Currency Palladium
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XPD");
            }
        }

        /// <summary>
        /// Gets the pseudo-currency for platinum.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PT.</remarks>
        /// <value>The pseudo-currency for platinum.</value>
        public static Currency Platinum
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XPT");
            }
        }

        /// <summary>
        /// Gets the pseudo-currency for silver.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AG.</remarks>
        /// <value>The pseudo-currency for silver.</value>
        public static Currency Silver
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);
                return Of("XAG");
            }
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

        /// <copydoc cref="IEquatable{T}.Equals" />
        public bool Equals(Currency other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                // Remember, "this" is never null in C#.
                return false;
            }

            return this._code == other._code;
        }

        /// <copydoc cref="Object.Equals(Object)" />
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

        /// <copydoc cref="Object.GetHashCode" />
        public override int GetHashCode()
        {
            return _code.GetHashCode();
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_code != null);
        }

#endif
    }
}
