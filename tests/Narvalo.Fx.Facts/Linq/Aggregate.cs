// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("Aggregate() guards.")]
        public static void Aggregate0() {
            IEnumerable<int> nullsource = null;
            Func<int, int, int> accumulator = (i, j) => i + j;
            Func<int, int> resultSelector = i => i + 1; ;
            Func<int, bool> predicate = _ => true;

            Assert.Throws<ArgumentNullException>(
                "this", () => nullsource.Aggregate(accumulator, predicate));
            Assert.Throws<ArgumentNullException>(
                "this", () => nullsource.Aggregate(1, accumulator, predicate));
            Assert.Throws<ArgumentNullException>(
                "this", () => nullsource.Aggregate(1, accumulator, resultSelector, predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>(
                "accumulator", () => source.Aggregate(null, predicate));
            Assert.Throws<ArgumentNullException>(
                "predicate", () => source.Aggregate(accumulator, null));

            Assert.Throws<ArgumentNullException>(
                "accumulator", () => source.Aggregate(1, null, predicate));
            Assert.Throws<ArgumentNullException>(
                "predicate", () => source.Aggregate(1, accumulator, null));

            Assert.Throws<ArgumentNullException>(
                "accumulator", () => source.Aggregate(1, null, resultSelector, predicate));
            Assert.Throws<ArgumentNullException>(
                "resultSelector", () => source.Aggregate(1, accumulator, default(Func<int, int>), predicate));
            Assert.Throws<ArgumentNullException>(
                "predicate", () => source.Aggregate(1, accumulator, resultSelector, null));
        }
    }
}
