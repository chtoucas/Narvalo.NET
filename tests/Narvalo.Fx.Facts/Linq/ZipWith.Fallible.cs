// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Fallible).")]
        public static void ZipWith1b() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Fallible<int>> resultSelector = (i, j) => Fallible.Of(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }
}
