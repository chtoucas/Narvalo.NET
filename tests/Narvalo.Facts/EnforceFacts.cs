// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class EnforceFacts
    {
        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
            => Enforce.NotNullOrWhiteSpace("value", "paramName");

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Enforce.NotNullOrWhiteSpace(null, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Enforce.NotNullOrWhiteSpace(String.Empty, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Enforce.NotNullOrWhiteSpace(My.WhiteSpaceOnlyString, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion

        #region PropertyNotWhiteSpace()

        [Fact]
        public static void PropertyNotWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
                => Enforce.PropertyNotWhiteSpace("value");

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Arrange
            Action act = () => Enforce.PropertyNotWhiteSpace(null);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("value", argex.ParamName);
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Arrange
            Action act = () => Enforce.PropertyNotWhiteSpace(String.Empty);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal("value", argex.ParamName);
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Arrange
            Action act = () => Enforce.PropertyNotWhiteSpace(My.WhiteSpaceOnlyString);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal("value", argex.ParamName);
        }

        #endregion

        #region NotWhiteSpace()

        [Fact]
        public static void NotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
            => Assert.Throws<ArgumentException>(() => Enforce.NotWhiteSpace(My.WhiteSpaceOnlyString, "paramName"));

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForNull()
            => Enforce.NotWhiteSpace(null, "paramName");

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForEmptyString()
            => Enforce.NotWhiteSpace(String.Empty, "paramName");

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForNonWhiteSpaceString()
            => Enforce.NotWhiteSpace("Whatever", "paramName");

        #endregion

        #region IsWhiteSpace()

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNull()
                => Assert.False(Enforce.IsWhiteSpace(null));

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForEmptyString()
            => Assert.False(Enforce.IsWhiteSpace(String.Empty));

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNonWhiteSpaceOnlyString()
            => Assert.False(Enforce.IsWhiteSpace("Whatever"));

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
            => Assert.True(Enforce.IsWhiteSpace(My.WhiteSpaceOnlyString));

        #endregion
    }
}
