// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("Fold() guards (Either).")]
        public static void Fold0a() {
            int seed = 0;
            Func<int, int, Either<int, int>> accumulator = (i, j) => Either<int, int>.OfLeft(i + j);
            Func<Either<int, int>, bool> predicate = _ => true;

            IEnumerable<int> nullsource = null;
            Assert.Throws<ArgumentNullException>("this", () => nullsource.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Either<int, int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }
}
