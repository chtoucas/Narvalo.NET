// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public sealed partial class CheckFacts : IClassFixture<DebugAssertFixture>
    {
        #region AssumeInvariant()

        [Fact]
        public static void AssumeInvariant_DoesNothing() => Check.AssumeInvariant(new Object());

        #endregion
    }
}
