// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Maybe).")]
        public static void ZipWith1c() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Maybe<int>> resultSelector = (i, j) => Maybe.Of(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnSome(x => Assert.ThrowsOnNext(x));
        }
    }
}
