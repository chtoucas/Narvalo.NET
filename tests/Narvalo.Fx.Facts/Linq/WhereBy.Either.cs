// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Either).")]
        public static void WhereBy0a() {
            IEnumerable<int> nullsource = null;
            Func<int, Either<bool, int>> predicate = _ => Either<bool, int>.OfLeft(true);

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Either<bool, int>>)));
        }

        [t("WhereBy() uses deferred execution (Either).")]
        public static void WhereBy1a() {
            bool notCalled = true;
            Func< Either<bool, int>> fun
                = () => { notCalled = false; return Either<bool, int>.OfLeft(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
        }
    }
}
