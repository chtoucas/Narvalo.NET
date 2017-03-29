// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("Reduce() guards (Maybe).")]
        public static void Reduce0c() {
            Func<int, int, Maybe<int>> accumulator = (i, j) => Maybe.Of(i + j);
            Func<Maybe<int>, bool> predicate = _ => true;

            IEnumerable<int> nullsource = null;
            Assert.Throws<ArgumentNullException>("this", () => nullsource.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Maybe<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }
}
