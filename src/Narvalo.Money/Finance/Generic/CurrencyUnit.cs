// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System;
    using System.Reflection;

    public partial class CurrencyUnit
    {
        private const string SINGLETON_PROPERTY_NAME = "Unit";

        // NB: On full .NET, there is a much faster alternative, namely:
        //   Activator.CreateInstance(typeof(TCurrency), nonPublic: true) as TCurrency;
        // Nevertheless, calling the constructor somehow defeats the idea of a unit being a singleton.
        internal static TCurrency OfType<TCurrency>() where TCurrency : Currency<TCurrency>
        {
            TypeInfo typeInfo = typeof(TCurrency).GetTypeInfo();

            // We expect that all built-in currency units defines this property.
            PropertyInfo property = typeInfo.GetDeclaredProperty(SINGLETON_PROPERTY_NAME);

            var currency = property?.GetValue(null) as TCurrency;

            if (currency == null) { throw new NotSupportedException("XXX"); }

            return currency;
        }
    }

    // Aliases for some of the most commonly used currencies.
    public static partial class CurrencyUnit
    {
        /// <summary>
        /// Gets the currency unit for the pseudo-currency for transactions where no currency is involved.
        /// </summary>
        public static XXX None => XXX.Unit;

        /// <summary>
        /// Gets the currency unit for the currency specifically reserved for testing purposes.
        /// </summary>
        public static XTS Test => XTS.Unit;

        /// <summary>
        /// Gets the currency unit for the "Euro".
        /// </summary>
        public static EUR Euro => EUR.Unit;

        /// <summary>
        /// Gets the currency unit for the "(British) "Pound Sterling".
        /// </summary>
        public static GBP PoundSterling => GBP.Unit;

        /// <summary>
        /// Gets the currency unit for the "Swiss Franc".
        /// </summary>
        public static CHF SwissFranc => CHF.Unit;

        /// <summary>
        /// Gets the currency unit for the "United States Dollar".
        /// </summary>
        public static USD UnitedStatesDollar => USD.Unit;

        /// <summary>
        /// Gets the currency unit for the "Japanese Yen".
        /// </summary>
        public static JPY Yen => JPY.Unit;

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for gold.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AU.</remarks>
        public static XAU Gold => XAU.Unit;

        /// <summary>
        /// Gets the currency unit class for the pseudo-currency for palladium.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PD.</remarks>
        public static XPD Palladium => XPD.Unit;

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for platinum.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PT.</remarks>
        public static XPT Platinum => XPT.Unit;

        /// <summary>
        /// Gets the currency unit for the pseudo-currency for silver.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AG.</remarks>
        public static XAG Silver => XAG.Unit;
    }
}
