// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class OutputFacts
    {
        #region Explicit Cast

        [Fact]
        public static void ExplicitCastToExceptionDispatchInfo_ThrowsInvalidCastException_WhenCastingSuccessObject()
        {
            // Arrange
            var output = Output.Success(1);

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => (ExceptionDispatchInfo)output);
        }

        [Fact]
        public static void ExplicitCastToValue_ThrowsInvalidCastException_WhenCastingFailureObject()
        {
            // Arrange
            Output<string> output = null;

            try
            {
                throw new SimpleException();
            }
            catch (SimpleException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                output = Output.Failure<string>(edi);
            }

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => (string)output);
        }

        #endregion
    }
}
