﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereAny() guards.")]
        public static void WhereAny0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereAny(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, bool?>)));
        }

        [t("WhereAny() uses deferred execution.")]
        public static void WhereAny1() {
            bool notCalled = true;
            Func<bool?> fun = () => { notCalled = false; return true; };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
        }
    }
}
