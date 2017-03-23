// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static class QperatorsFacts
    {
        #region Append()

        [Fact]
        public static void Append_Guards()
        {
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Append(1));
        }

        //[Fact]
        //public static void Append_IsDeferred() => Assert.IsDeferred(src => src.Append(1));

        #endregion

        #region Prepend()

        [Fact]
        public static void Prepend_Guards()
        {
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Prepend(1));
        }

        //[Fact]
        //public static void Prepend_IsDeferred() => Assert.IsDeferred(src => src.Prepend(1));

        #endregion

        #region SelectAny()

        [Fact]
        public static void SelectAny_Guards()
        {
            IEnumerable<int> source = null;
            Func<int, int?> selector = val => val;
            Assert.Throws<ArgumentNullException>(() => source.SelectAny(selector));
            Assert.Throws<ArgumentNullException>(() => source.SelectAny(val => String.Empty));
            Assert.Throws<ArgumentNullException>(() => source.SelectAny((Func<int, int?>)null));
            Assert.Throws<ArgumentNullException>(() => source.SelectAny((Func<int, string>)null));
        }

        [Fact]
        public static void SelectAny_IsDeferred()
        {
            Func<int, int?> selector = val => val;
            Assert.IsDeferred(src => src.SelectAny(selector));
            Assert.IsDeferred(src => src.SelectAny(val => String.Empty));
        }

        #endregion
    }
}
