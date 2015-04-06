// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    using Narvalo.TestCommon;
    using Xunit;

    public static class VoidOrErrorFacts
    {
        #region IsError

        [Fact]
        public static void IsError_IsFalse_ForVoidObject()
        {
            // Act & Assert
            Assert.False(VoidOrError.Void.IsError);
        }

        [Fact]
        public static void IsError_IsTrue_ForErrorObject()
        {
            // Arrange
            VoidOrError voe = null;

            // Act
            try
            {
                throw new SimpleException();
            }
            catch (SimpleException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                voe = VoidOrError.Error(edi);
            }

            // Assert
            Assert.True(voe.IsError);
        }

        #endregion

        #region Break

        [Fact]
        public static void ThrowIfError_DoesNotThrow_ForVoidObject()
        {
            // Act
            VoidOrError.Void.ThrowIfError();
        }

        [Fact]
        public static void ThrowIfError_ThrowsOriginalException_ForErrorObject()
        {
            // Arrange
            VoidOrError voe = null;

            // Act
            try
            {
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                voe = VoidOrError.Error(edi);
            }

            // Assert
            Assert.Throws<InvalidOperationException>(() => voe.ThrowIfError());
        }

        #endregion
    }
}
