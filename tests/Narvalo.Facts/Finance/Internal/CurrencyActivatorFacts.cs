// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Finance.Currencies;
    using Xunit;

    public static partial class CurrencyActivatorFacts
    {
        #region CreateInstance()

        [Fact]
        public static void CreateInstance_Passes_BuiltInCurrency()
            => CurrencyActivator<EUR>.CreateInstance();

        [Fact]
        public static void CreateInstance_Passes_PublicCtor()
            => CurrencyActivator<MyCurrency_>.CreateInstance();

        [Fact]
        public static void CreateInstance_Passes_PrivateCtor()
            => CurrencyActivator<MyCurrencyWithPrivateCtor_>.CreateInstance();

        [Fact]
        public static void CreateInstance_ThrowsMissingMemberException_MissingDefaultCtor()
            => Assert.Throws<MissingMemberException>(
                () => CurrencyActivator<MyCurrencyWithoutDefaultCtor_>.CreateInstance());

        #endregion
    }

    // Helpers.
    public static partial class CurrencyActivatorFacts
    {
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "[Intentionally] Testing dynamic contruction of a class.")]
        private class MyCurrency_ : Currency
        {
            public MyCurrency_() : base("YYY") { }
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "[Intentionally] Testing dynamic contruction of a class.")]
        private class MyCurrencyWithPrivateCtor_ : Currency
        {
            private MyCurrencyWithPrivateCtor_() : base("YYY") { }
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "[Intentionally] Testing dynamic contruction of a class.")]
        private class MyCurrencyWithoutDefaultCtor_ : Currency
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Testing a class without public ctor.")]
            private MyCurrencyWithoutDefaultCtor_(string value) : base("YYY") { }
        }
    }

#endif
}
