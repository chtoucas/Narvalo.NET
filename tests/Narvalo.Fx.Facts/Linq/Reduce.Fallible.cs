// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("Reduce() guards (Fallible).")]
        public static void Reduce0b() {
            Func<int, int, Fallible<int>> accumulator = (i, j) => Fallible.Of(i + j);
            Func<Fallible<int>, bool> predicate = _ => true;

            IEnumerable<int> nullsource = null;
            Assert.Throws<ArgumentNullException>("this", () => nullsource.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Fallible<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }
}
