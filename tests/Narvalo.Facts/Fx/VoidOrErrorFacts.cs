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
        public static void IsError_ReturnsFalse_WithVoidObject()
        {
            // Act & Assert
            Assert.False(VoidOrError.Void.IsError);
        }

        [Fact]
        public static void IsError_ReturnsTrue_WithErrorObject()
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

        #region ThrowIfError()

        [Fact]
        public static void ThrowIfError_DoesNotThrow_WithVoidObject()
        {
            // Act
            VoidOrError.Void.ThrowIfError();
        }

        [Fact]
        public static void ThrowIfError_ThrowsOriginalException_WithErrorObject()
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
            Assert.Throws<SimpleException>(() => voe.ThrowIfError());
        }

        #endregion

        #region ToString()

        [Fact]
        public static void ToString_IsOverridden_WithVoidObject()
        {
            // Act
            Assert.Equal("Void", VoidOrError.Void.ToString());
        }

        [Fact]
        public static void ToString_ContainsExceptionMessage_WithErrorObject()
        {
            // Arrange
            VoidOrError voe = null;
            var message = "My exception message.";

            // Act
            try
            {
                throw new SimpleException(message);
            }
            catch (SimpleException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                voe = VoidOrError.Error(edi);
            }

            // Assert
            Assert.True(voe.ToString().Contains(message));
        }

        #endregion
    }
}
