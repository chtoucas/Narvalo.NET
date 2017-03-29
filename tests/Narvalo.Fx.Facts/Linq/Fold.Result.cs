// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("Fold() guards (Result).")]
        public static void Fold0e() {
            int seed = 0;
            Func<int, int, Result<int, int>> accumulator = (i, j) => Result<int, int>.Of(i + j);
            Func<Result<int, int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("this", () => nil.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Result<int, int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }
}
