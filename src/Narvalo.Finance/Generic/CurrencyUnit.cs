// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public static partial class CurrencyUnit
    {
        private const string SingletonPropertyName = "Unit";

        // NB: On full .NET, there is a much faster alternative, namely:
        //   Activator.CreateInstance(typeof(TCurrency), nonPublic: true) as TCurrency;
        // Nevertheless, calling the constructor somehow defeats the idea of a unit being a singleton.
        internal static TCurrency OfType<TCurrency>() where TCurrency : CurrencyUnit<TCurrency>
        {
            Warrant.NotNull<TCurrency>();

            var typeInfo = typeof(TCurrency).GetTypeInfo();
            Contract.Assume(typeInfo != null);

            // We expect that all built-in currency units defines this property.
            var property = typeInfo.GetDeclaredProperty(SingletonPropertyName);

            return property?.GetValue(null) as TCurrency;
        }
    }

    // Aliases for some of the most commonly used currencies.
    public static partial class CurrencyUnit
    {
        /// <summary>
        /// Gets the currency unit for the pseudo-currency for transactions where no currency is involved.
        /// </summary>
        public static XXX None { get { Warrant.NotNull<XXX>(); return XXX.Unit; } }

        /// <summary>
        /// Gets the currency unit for the currency specifically reserved for testing purposes.
        /// </summary>
        public static XTS Test { get { Warrant.NotNull<XTS>(); return XTS.Unit; } }

        /// <summary>
        /// Gets the currency unit for the "Euro".
        /// </summary>
        public static EUR Euro { get { Warrant.NotNull<EUR>(); return EUR.Unit; } }

        /// <summary>
        /// Gets the currency unit for the "(British) "Pound Sterling".
        /// </summary>
        public static GBP PoundSterling { get { Warrant.NotNull<GBP>(); return GBP.Unit; } }

        /// <summary>
        /// Gets the currency unit for the "Swiss Franc".
        /// </summary>
        public static CHF SwissFranc { get { Warrant.NotNull<CHF>(); return CHF.Unit; } }

        /// <summary>
        /// Gets the currency unit for the "United States Dollar".
        /// </summary>
        public static USD UnitedStatesDollar { get { Warrant.NotNull<USD>(); return USD.Unit; } }

        /// <summary>
        /// Gets the currency unit for the "Japanese Yen".
        /// </summary>
        public static JPY Yen { get { Warrant.NotNull<JPY>(); return JPY.Unit; } }

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for gold.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AU.</remarks>
        public static XAU Gold { get { Warrant.NotNull<XAU>(); return XAU.Unit; } }

        /// <summary>
        /// Gets the currency unit class for the pseudo-currency for palladium.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PD.</remarks>
        public static XPD Palladium { get { Warrant.NotNull<XPD>(); return XPD.Unit; } }

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for platinum.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PT.</remarks>
        public static XPT Platinum { get { Warrant.NotNull<XPT>(); return XPT.Unit; } }

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for silver.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AG.</remarks>
        public static XAG Silver { get { Warrant.NotNull<XAG>(); return XAG.Unit; } }
    }
}
