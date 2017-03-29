// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("SelectAny() guards (Maybe).")]
        public static void SelectAny0b() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.SelectAny(x => Fallible.Of(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Maybe<int>>)));
        }

        [t("SelectAny() uses deferred execution (Maybe).")]
        public static void SelectAny1b() {
            bool notCalled = true;
            Func<Maybe<int>> fun = () => { notCalled = false; return Maybe.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }
}
