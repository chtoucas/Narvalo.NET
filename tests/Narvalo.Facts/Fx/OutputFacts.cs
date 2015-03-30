// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    public static partial class OutputFacts
    {
        #region Explicit Cast

        [Fact]
        public static void ExplicitCast_Throws_WhenCastingSuccessToFailure()
        {
            // Arrange
            var output = Output.Success(1);

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => (ExceptionDispatchInfo)output);
        }

        #endregion
    }
}
