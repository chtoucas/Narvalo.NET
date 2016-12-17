// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Collections.Generic;
    using System.Globalization;

    using Narvalo.Finance.Currencies;
    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.Utilities.AsciiHelpers;

    /// <summary>
    /// Represents a currency unit such as Euro or US Dollar.
    /// </summary>
    /// <remarks>
    /// <para>Recognized currencies are defined in ISO 4217.</para>
    /// <para>There's never more than one <see cref="Currency"/> instance for any given currency.
    /// You can not directly construct a currency. You must instead use the
    /// static factories: <see cref="Currency.Of"/>.</para>
    /// <para>This class follows value type semantics when it comes to equality.</para>
    /// <para>This class does not offer extended information about the currency.</para>
    /// </remarks>
    public partial class Currency : IEquatable<Currency>
    {
        private volatile static HashSet<string> s_CodeSet;

        private readonly string _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency" /> class for the specified code.
        /// </summary>
        /// <param name="code">A string that contains the three-letter identifier defined in ISO 4217.</param>
        internal Currency(string code)
        {
            Sentinel.Demand.CurrencyCode(code);

            _code = code;
        }

        /// <summary>
        /// Gets the alphabetic code of the currency.
        /// </summary>
        /// <value>The alphabetic code of the currency.</value>
        public string Code
        {
            get { Warrant.NotNull<string>(); return _code; }
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
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);
            Warrant.NotNull<Currency>();

            if (!CodeSet.Contains(code))
            {
                throw new CurrencyNotFoundException(Format.Current(Strings.Currency_UnknownCurrency, code));
            }

            return new Currency(code);
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
            Require.NotNull(regionInfo, nameof(regionInfo));
            Warrant.NotNull<Currency>();

            var code = regionInfo.ISOCurrencySymbol;
            Contract.Assume(code != null);
            Contract.Assume(code.Length != 0);   // Should not be necessary, but CCCheck insists.
            Contract.Assume(IsUpperLetter(code));
            Contract.Assume(code.Length == 3);

            return new Currency(code);
        }

        public static Currency OfCurrentRegion()
        {
            Warrant.NotNull<Currency>();

            return OfRegion(RegionInfo.CurrentRegion);
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated
        /// with the specified culture.
        /// </summary>
        /// <param name="cultureInfo">A culture info.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="cultureInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the specified culture.</exception>
        /// <returns>The currency for the specified culture info.</returns>
        public static Currency OfCulture(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));
            Warrant.NotNull<Currency>();

            if (cultureInfo.IsNeutralCulture)
            {
                throw new NotSupportedException(Strings.Currency_NotSupportedNeutralCulture);
            }

            return OfRegion(new RegionInfo(cultureInfo.Name));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the culture
        /// used by the current thread.
        /// </summary>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the current culture.</exception>
        /// <returns>The currency for the culture used by the current thread.</returns>
        public static Currency OfCurrentCulture()
        {
            Warrant.NotNull<Currency>();

            return OfCulture(CultureInfo.CurrentCulture);
        }

        public static bool RegisterCurrency(string code)
        {
            Sentinel.Require.CurrencyCode(code, nameof(code));

            if (CodeSet.Contains(code)) { return false; }

            return CodeSet.Add(code);
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

    // Aliases for the most commonly used currencies.
    public partial class Currency
    {
        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class
        /// for the pseudo-currency for transactions where no currency is involved.
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class
        /// for the pseudo-currency for transactions where no currency is involved.</value>
        public static Currency None
        {
            get { Warrant.NotNull<Currency>(); return XXX.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class
        /// for the currency specifically reserved for testing purposes.
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class
        /// for the currency specifically reserved for testing purposes.</value>
        public static Currency Test
        {
            get { Warrant.NotNull<Currency>(); return XTS.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the "Euro".
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the "Euro".</value>
        public static Currency Euro
        {
            get { Warrant.NotNull<Currency>(); return EUR.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the "(British) "Pound Sterling".
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the (British) "Pound Sterling".</value>
        public static Currency PoundSterling
        {
            get { Warrant.NotNull<Currency>(); return GBP.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the "Swiss Franc".
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the "Swiss Franc".</value>
        public static Currency SwissFranc
        {
            get { Warrant.NotNull<Currency>(); return CHF.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the "United States Dollar".
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the "United States Dollar".</value>
        public static Currency UnitedStatesDollar
        {
            get { Warrant.NotNull<Currency>(); return USD.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the "Japanese Yen".
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the "Japanese Yen".</value>
        public static Currency Yen
        {
            get { Warrant.NotNull<Currency>(); return JPY.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the pseudo-currency for gold.
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the pseudo-currency for gold.</value>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AU.</remarks>
        public static Currency Gold
        {
            get { Warrant.NotNull<Currency>(); return XAU.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the pseudo-currency for palladium.
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the pseudo-currency for palladium.</value>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PD.</remarks>
        public static Currency Palladium
        {
            get { Warrant.NotNull<Currency>(); return XPD.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the pseudo-currency for platinum.
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the pseudo-currency for platinum.</value>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PT.</remarks>
        public static Currency Platinum
        {
            get { Warrant.NotNull<Currency>(); return XPT.UniqInstance; }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the pseudo-currency for silver.
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the pseudo-currency for silver.</value>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AG.</remarks>
        public static Currency Silver
        {
            get { Warrant.NotNull<Currency>(); return XAG.UniqInstance; }
        }
    }

    // Interface IEquatable<Currency>.
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

        public static bool operator !=(Currency left, Currency right) => !(left == right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Currency other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                // NB: "this" is never null in C#.
                return false;
            }

            return Code == other.Code;
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                // NB: "this" is never null in C#.
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                // "obj" and "this" are the same object.
                return true;
            }

            if (GetType() != obj.GetType())
            {
                // "obj" and "this" are not of the same type.
                return false;
            }

            return Equals(obj as Currency);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode() => Code.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public partial class Currency
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
