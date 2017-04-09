// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public partial class QperatorsFacts {
        [t("Aggregate() guards.")]
        public static void Aggregate0() {
            Func<int, int, int> accumulator = (i, j) => i + j;
            Func<int, bool> predicate = _ => true;

            IEnumerable<int> nil = null;

            Assert.Throws<ArgumentNullException>("source", () => nil.Aggregate(accumulator, predicate));
            Assert.Throws<ArgumentNullException>("source", () => nil.Aggregate(1, accumulator, predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("accumulator", () => source.Aggregate(null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Aggregate(accumulator, null));

            Assert.Throws<ArgumentNullException>("accumulator", () => source.Aggregate(1, null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Aggregate(1, accumulator, null));
        }
    }
}
