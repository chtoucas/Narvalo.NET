// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Tests;

    public partial class SequenceFacts : EnumerableTests {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Sequence), description) { }
        }
    }
}
