// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    using static Narvalo.Finance.Utilities.AsciiHelpers;

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
    public sealed partial class Currency : IEquatable<Currency>
    {
        // The list is automatically generated using the data obtained from the SNV website.
        // The volatile keyword is only for correctness.
        private volatile static HashSet<string> s_CodeSet;

        private readonly string _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency" /> class for the specified code.
        /// </summary>
        /// <param name="code">A string that contains the three-letter identifier defined in ISO 4217.</param>
        private Currency(string code)
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

            Contract.Assume(CodeSet != null);
            if (!CodeSet.Contains(code))
            {
                throw new CurrencyNotFoundException(Format.Current(Strings.Currency_UnknownCode, code));
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
        public static Currency OfCurrentCulture()
        {
            Warrant.NotNull<Currency>();

            return OfCulture(CultureInfo.CurrentCulture);
        }

        // This method allows to register currencies that are not part of ISO 4217.
        // See https://en.wikipedia.org/wiki/ISO_4217.
        // REVIEW: Should we worry about concurrent access? I'm not sure.
        // If the code is already registered, no problem, otherwise two threads may attempt to add
        // the same code; the first win, the second simply returns false?
        public static bool RegisterCurrency(string code)
        {
            Sentinel.Require.CurrencyCode(code, nameof(code));

            Contract.Assume(CodeSet != null);
            if (CodeSet.Contains(code)) { return false; }

            return CodeSet.Add(code);
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
    public sealed partial class Currency
    {
        // WARNING: This (immutable) reference type follows value type semantics.
        // To test reference equality, you must use Object.ReferenceEquals().
        public static bool operator ==(Currency left, Currency right)
        {
            if (ReferenceEquals(left, null))
            {
                // If left is null, returns true if right is null too, otherwise false.
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Currency left, Currency right) => !(left == right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Currency other)
        {
            if (ReferenceEquals(other, null)) { return false; }

            return EqualsImpl(other);
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) { return false; }
            if (ReferenceEquals(obj, this)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            return EqualsImpl(obj as Currency);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        // TODO: Since there are so few currencies, we could cache the hash code?
        public override int GetHashCode() => Code.GetHashCode();

        private bool EqualsImpl(Currency other)
        {
            Demand.NotNull(other);

            return Code == other.Code;
        }
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
