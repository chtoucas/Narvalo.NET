// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using Xunit;

    internal class TestCaseAttribute : FactAttribute {
        public TestCaseAttribute(string category, string description) : base() {
            DisplayName = category + " - " + description;
        }
    }
}
