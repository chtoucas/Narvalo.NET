// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class HashCodeHelpersFacts
    {
        [Fact]
        public static void Combine_DoesNotThrow_WhenOverflow()
        {
            HashCodeHelpers.Combine(Int32.MinValue, 1);
            HashCodeHelpers.Combine(Int32.MaxValue, 1);
            HashCodeHelpers.Combine(Int32.MinValue, 1, 1);
            HashCodeHelpers.Combine(Int32.MaxValue, 1, 1);
        }
    }
}
