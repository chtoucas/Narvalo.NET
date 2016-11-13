// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class ContractHelpersFacts
    {
        #region AssumeInvariant()

        [Fact]
        public static void Invariant_DoesNothing()
        {
            // Arrange a Act
            ContractHelpers.AssumeInvariant(new Object());
        }

        #endregion
    }
}
