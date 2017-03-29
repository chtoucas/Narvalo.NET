// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {

        [t("SelectWith() uses deferred execution (Outcome).")]
        public static void SelectWith1d() {
            bool notCalled = true;
            Func<Outcome<int>> fun = () => { notCalled = false; return Outcome.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
        }
    }
}
