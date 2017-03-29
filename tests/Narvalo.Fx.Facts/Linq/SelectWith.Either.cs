// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Either).")]
        public static void SelectWith1a() {
            bool notCalled = true;
            Func<Either<int, int>> fun = () => { notCalled = false; return Either<int, int>.OfLeft(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnLeft(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnRight(x => Assert.Fail());
        }
    }
}
