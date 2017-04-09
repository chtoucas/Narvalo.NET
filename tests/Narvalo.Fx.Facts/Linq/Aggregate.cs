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

            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(1, accumulator, predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));

            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(1, null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(1, accumulator, null));
        }
    }
}
