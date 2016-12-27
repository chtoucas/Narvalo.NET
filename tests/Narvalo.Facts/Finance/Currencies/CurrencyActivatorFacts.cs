// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Currencies
{
#if !NO_INTERNALS_VISIBLE_TO

    using System.Reflection;

    using Xunit;

    public static partial class CurrencyActivatorFacts
    {
        #region CreateInstance()

        [Fact]
        public static void CreateInstance_Passes_BuiltInUnit()
            => CurrencyActivator.CreateInstance<EUR>();

        [Fact]
        public static void CreateInstance_Passes_CustomUnit_PublicCtor()
            => CurrencyActivator.CreateInstance<ZZZ>();

        [Fact]
        public static void CreateInstance_ThrowsTargetParameterCountException_ForMissingDefaultCtor()
            => Assert.Throws<TargetParameterCountException>(() => CurrencyActivator.CreateInstance<VVV>());

        #endregion
    }

    // Helpers.
    public static partial class CurrencyActivatorFacts
    {
        private sealed class ZZZ : CurrencyUnit<ZZZ> { public ZZZ() { } }

        private sealed class VVV : CurrencyUnit<VVV> { public VVV(string code) { } }
    }

#endif
}
