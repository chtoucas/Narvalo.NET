// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using Xunit;

    internal class FactAttribute_ : FactAttribute {
        private const string SEP = " - ";

        public FactAttribute_(string category, string message) : base() {
            DisplayName = category + SEP + message;
        }
    }
}
