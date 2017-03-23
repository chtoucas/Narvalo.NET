// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static class SelectAnyFacts
    {
        [Fact]
        public static void Guards()
        {
            IEnumerable<int> source = null;
            Func<int, int?> selector = val => val;

            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(selector));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(val => String.Empty));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny((Func<int, int?>)null));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny((Func<int, string>)null));
        }

        [Fact]
        public static void IsDeferred()
        {
            Func<int, int?> selector = val => val;

            Assert.IsDeferred(src => src.SelectAny(selector));
            Assert.IsDeferred(src => src.SelectAny(val => String.Empty));
        }
    }
}
