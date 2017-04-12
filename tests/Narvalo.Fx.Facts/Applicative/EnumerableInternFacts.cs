// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_INTERNALS_VISIBLE_TO

namespace Narvalo.Applicative {
    using System.Linq.Tests;

    public partial class EnumerableInternFacts : EnumerableTests {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(EnumerableIntern), description) { }
        }
    }
}

#endif