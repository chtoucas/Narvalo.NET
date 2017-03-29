// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Fallible).")]
        public static void SelectWith1b() {
            bool notCalled = true;
            Func<Fallible<int>> fun = () => { notCalled = false; return Fallible.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }
}
