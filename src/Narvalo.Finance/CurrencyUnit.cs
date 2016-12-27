// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using Narvalo.Finance.Currencies;

    // Aliases for the most commonly used currencies.
    public static partial class CurrencyUnit
    {
        /// <summary>
        /// Gets he currency unit for the pseudo-currency for transactions where no currency is involved.
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
