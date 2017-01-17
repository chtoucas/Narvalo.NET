// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
#if !NO_INTERNALS_VISIBLE_TO

    using Narvalo.Finance.Generic;
    using Xunit;

    public static partial class CurrencyUnitFacts
    {
        #region OfType()

        [Fact]
        public static void OfType_Passes_ForBuiltInUnit()
            => CurrencyUnit.OfType<EUR>();

        [Fact]
        public static void OfType_ReturnsNull_ForMissingSingletonProperty()
            => Assert.Null(CurrencyUnit.OfType<XXN>());

        #endregion

        private sealed class XXN : Currency<XXN> { private XXN() : base(0) { } }
    }

#endif
}
