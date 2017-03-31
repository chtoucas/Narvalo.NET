// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("SelectAny() guards (Either).")]
        public static void SelectAny0a() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.SelectAny(x => Either< int, int>.OfLeft(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Either<int, int>>)));
        }

        [t("SelectAny() uses deferred execution (Either).")]
        public static void SelectAny1a() {
            bool notCalled = true;
            Func<Either<int, int>> fun = () => { notCalled = false; return Either<int, int>.OfLeft(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }
}
