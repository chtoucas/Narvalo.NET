// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using Xunit;

    public class TestTheoryAttribute : TheoryAttribute {
        public TestTheoryAttribute(string category, string description) : base() {
            DisplayName = category + " - " + description;
        }
    }
}
