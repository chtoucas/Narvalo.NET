// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class VoidOrErrorFacts
    {
        #region Void

        [Fact]
        public static void Void_IsNotNull()
        {
            // Act & Assert
            Assert.True(VoidOrError.Void != null);
        }

        #endregion

        #region IsError

        [Fact]
        public static void IsError_ReturnsFalse_WhenVoid()
        {
            // Act & Assert
            Assert.False(VoidOrError.Void.IsError);
        }

        [Fact]
        public static void IsError_ReturnsTrue_WhenError()
        {
            // Arrange
            var edi = CreateEdi_();
            var voe = VoidOrError.Error(edi);

            // Act & Assert
            Assert.True(voe.IsError);
        }

        #endregion

        #region Error()

        [Fact]
        public static void Error_ThrowsArgumentNullException_ForNullInput()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => VoidOrError.Error(null));
        }

        [Fact]
        public static void Error_ResultIsNotNull()
        {
            // Act
            var edi = CreateEdi_();
            var voe = VoidOrError.Error(edi); 

            // Assert
            Assert.True(voe != null);
        }

        #endregion

        #region ThrowIfError()

        [Fact]
        public static void ThrowIfError_DoesNotThrow_WhenVoid()
        {
            // Act
            VoidOrError.Void.ThrowIfError();
        }

        [Fact]
        public static void ThrowIfError_ThrowsOriginalException_WhenError()
        {
            // Arrange
            var edi = CreateEdiFrom_(new SimpleException());
            var voe = VoidOrError.Error(edi); 

            // Act & Assert
            Assert.Throws<SimpleException>(() => voe.ThrowIfError());
        }

        #endregion

        #region ToString()

        [Fact]
        public static void ToString_ReturnsVoid_WhenVoid()
        {
            // Act & Assert
            Assert.Equal("Void", VoidOrError.Void.ToString());
        }

        [Fact]
        public static void ToString_ResultIsNotNull_WhenError()
        {
            // Arrange
            var edi = CreateEdi_();
            var voe = VoidOrError.Error(edi); 

            // Act a Assert
            Assert.True(voe.ToString() != null);
        }

        [Fact]
        public static void ToString_ResultContainsExceptionMessage_WhenError()
        {
            // Arrange
            var message = "My exception message.";
            var edi = CreateEdiFrom_(new SimpleException(message));
            var voe = VoidOrError.Error(edi);

            // Act & Assert
            Assert.True(voe.ToString().Contains(message));
        }

        #endregion
    }

    public static partial class VoidOrErrorFacts
    {
        private static ExceptionDispatchInfo CreateEdi_()
        {
            return CreateEdiFrom_(new SimpleException());
        }

        private static ExceptionDispatchInfo CreateEdiFrom_(Exception exception)
        {
            ExceptionDispatchInfo edi;

            try
            {
                throw exception;
            }
            catch (SimpleException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            return edi;
        }
    }
}
