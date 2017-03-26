// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;

    public static class HashCodeHelpersFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(HashCodeHelpers), message) { }
        }

        [fact("Combine() does not throw OverflowException.")]
        public static void Combine() {
            HashCodeHelpers.Combine(Int32.MinValue, 1);
            HashCodeHelpers.Combine(Int32.MaxValue, 1);
            HashCodeHelpers.Combine(Int32.MinValue, 1, 1);
            HashCodeHelpers.Combine(Int32.MaxValue, 1, 1);
        }
    }
}
