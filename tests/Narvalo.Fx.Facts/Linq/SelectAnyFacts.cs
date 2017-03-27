// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static class SelectAnyFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Qperators), description) { }
        }

        [t("")]
        public static void Guards() {
            IEnumerable<int> source = null;
            Func<int, int?> selector = val => val;

            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(selector));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(val => String.Empty));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny((Func<int, int?>)null));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny((Func<int, string>)null));
        }

        [t("")]
        public static void IsDeferred_1() {
            bool wasCalled = false;
            Func<string> fun = () => { wasCalled = true; return String.Empty; };

            var q = Enumerable.Repeat(fun, 1).SelectAny(val => val());

            Assert.False(wasCalled);
        }

        [t("")]
        public static void IsDeferred_2() {
            bool wasCalled = false;
            Func<int?> fun = () => { wasCalled = true; return 1; };

            var q = Enumerable.Repeat(fun, 1).SelectAny(val => val());

            Assert.False(wasCalled);
        }
    }
}
