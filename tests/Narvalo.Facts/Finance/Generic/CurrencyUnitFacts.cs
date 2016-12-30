// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class CurrencyUnitFacts
    {
        #region Aliases

        [Theory]
        [MemberData(nameof(Aliases), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Alias_IsNotNull(object alias) => Assert.NotNull(alias);

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class CurrencyUnitFacts
    {
        #region OfType()

        [Fact]
        public static void OfType_Passes_ForBuiltInUnit()
            => CurrencyUnit.OfType<EUR>();

        [Fact]
        public static void OfType_ReturnsNull_ForMissingSingletonProperty()
            => Assert.Null(CurrencyUnit.OfType<ZZZ>());

        #endregion

        private sealed class ZZZ : CurrencyUnit<ZZZ> { private ZZZ() { } }
    }

#endif

    public static partial class CurrencyUnitFacts
    {
        public static IEnumerable<object[]> Aliases
        {
            get
            {
                yield return new object[] { CurrencyUnit.None };
                yield return new object[] { CurrencyUnit.Test };
                yield return new object[] { CurrencyUnit.Euro };
                yield return new object[] { CurrencyUnit.PoundSterling };
                yield return new object[] { CurrencyUnit.SwissFranc };
                yield return new object[] { CurrencyUnit.UnitedStatesDollar };
                yield return new object[] { CurrencyUnit.Yen };
                yield return new object[] { CurrencyUnit.Gold };
                yield return new object[] { CurrencyUnit.Palladium };
                yield return new object[] { CurrencyUnit.Platinum };
                yield return new object[] { CurrencyUnit.Silver };
            }
        }
    }
}
