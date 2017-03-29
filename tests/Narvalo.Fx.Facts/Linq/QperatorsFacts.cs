// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System.Collections.Generic;
    using System.Linq.Tests;

    public partial class QperatorsFacts : EnumerableTests {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Qperators), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(Qperators), description) { }
        }

        private static IEnumerable<T> EmptySource<T>() {
            yield break;
        }
    }
}
