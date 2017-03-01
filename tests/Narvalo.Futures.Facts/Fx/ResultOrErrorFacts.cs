// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    public static partial class ResultOrErrorFacts
    {
        #region Explicit Cast

        //[Fact]
        //public static void ExplicitCastToExceptionDispatchInfo_ThrowsInvalidCastException_WhenCastingSuccessObject()
        //{
        //    // Arrange
        //    var output = ResultOrError.Of(1);

        //    // Act & Assert
        //    Assert.Throws<InvalidCastException>(() => (ExceptionDispatchInfo)output);
        //}

        [Fact]
        public static void ExplicitCastToValue_ThrowsInvalidCastException_WhenCastingFailureObject()
        {
            // Arrange
            Result<string> output = null;

            try
            {
                throw new My.SimpleException();
            }
            catch (My.SimpleException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                output = ResultOrError.FromError<string>(edi);
            }

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => (string)output);
        }

        #endregion
    }
}
