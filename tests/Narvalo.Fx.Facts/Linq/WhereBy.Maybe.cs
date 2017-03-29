// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Maybe).")]
        public static void WhereBy0c() {
            IEnumerable<int> nullsource = null;
            Func<int, Maybe<bool>> predicate = _ => Maybe.Of(true);

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Maybe<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Maybe).")]
        public static void WhereBy1c() {
            bool notCalled = true;
            Func<Maybe<bool>> fun = () => { notCalled = false; return Maybe.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
        }
    }
}
