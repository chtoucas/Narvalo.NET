// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class RequireFacts
    {
        #region State()

        [Fact]
        public static void State_DoesNotThrow_ForTrue_1() => Require.State(true);

        [Fact]
        public static void State_DoesNotThrow_ForTrue_2() => Require.State(true, "My message");

        [Fact]
        public static void State_ThrowsInvalidOperationException_ForFalse_1()
            => Assert.Throws<InvalidOperationException>(() => Require.State(false));

        [Fact]
        public static void State_ThrowsInvalidOperationException_ForFalse_2()
        {
            // Arrange
            var message = "My message";
            Action act = () => Require.State(false, message);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal(message, ex.Message);
        }

        #endregion

        #region True()

        [Fact]
        public static void True_DoesNotThrow_ForTrue_1() => Require.True(true, "paramName");

        [Fact]
        public static void True_DoesNotThrow_ForTrue_2() => Require.True(true, "paramName", "My message");

        [Fact]
        public static void True_ThrowsArgumentException_ForFalse_1()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Require.True(false, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void True_ThrowsArgumentException_ForFalse_2()
        {
            // Arrange
            var paramName = "paramName";
            var message = "My message";
            Action act = () => Require.True(false, paramName, message);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
            // ArgumentException appends some info to our message.
            Assert.StartsWith(message, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Range()

        [Fact]
        public static void Range_DoesNotThrow_ForTrue_1() => Require.Range(true, "paramName");

        [Fact]
        public static void Range_DoesNotThrow_ForTrue_2() => Require.Range(true, "paramName", "My message");

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForFalse_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.Range(false, "paramName"));
            // Arrange
            var paramName = "paramName";
            Action act = () => Require.Range(false, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForFalse_2()
        {
            // Arrange
            var paramName = "paramName";
            var message = "My message";
            Action act = () => Require.Range(false, paramName, message);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
            // NB: ArgumentOutOfRangeException appends some info to our message.
            Assert.StartsWith(message, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull() => Require.NotNull(new Object(), "paramName");

        [Fact]
        public static void NotNull_ThrowsArgumentNullException_ForNull()
        {
            // Arrange
            Object obj = null;
            var paramName = "paramName";
            Action act = () => Require.NotNull(obj, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion

        #region NotNullUnconstrained()

        [Fact]
        public static void NotNullUnconstrained_DoesNotThrow_ForStruct()
            => Require.NotNullUnconstrained(new My.EmptyStruct(), "paramName");

        [Fact]
        public static void NotNullUnconstrained_DoesNotThrow_ForNonNull()
            => Require.NotNullUnconstrained(new Object(), "paramName");

        [Fact]
        public static void NotNullUnconstrained_ThrowsArgumentNullException_ForNull()
        {
            // Arrange
            Object obj = null;
            var paramName = "paramName";
            Action act = () => Require.NotNullUnconstrained(obj, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
            => Require.NotNullOrEmpty("value", "paramName");

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentNullException_ForNull()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Require.NotNullOrEmpty(null, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Require.NotNullOrEmpty(String.Empty, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion
    }
}
