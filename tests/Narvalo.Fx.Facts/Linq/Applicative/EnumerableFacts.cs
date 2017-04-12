// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative {
    using System.Linq.Tests;

    public partial class EnumerableFacts : EnumerableTests {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base("Aperators", description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base("Aperators", description) { }
        }
    }
}
