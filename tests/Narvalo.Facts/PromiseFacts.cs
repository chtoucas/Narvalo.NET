// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class PromiseFacts
    {
        private const string NULL_STRING = null;
        private const string WHITESPACE_ONLY_STRING = "     ";

        #region Condition()

        [Fact]
        public static void Condition_DoesNotThrow_ForTrueCondition()
        {
            // Act
            Promise.Condition(true, "rationale");
        }

        [Fact]
        public static void Condition_Throws_ForFalseCondition()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.Condition(false, "rationale"));
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull()
        {
            // Act
            Promise.NotNull(String.Empty, "rationale");
        }

        [Fact]
        public static void NotNull_Throws_ForNull()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNull(NULL_STRING, "rationale"));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Promise.NotNullOrEmpty("value", "rationale");
        }

        [Fact]
        public static void NotNullOrEmpty_Throws_ForNull()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrEmpty(NULL_STRING, "rationale"));
        }

        [Fact]
        public static void NotNullOrEmpty_Throws_ForEmptyString()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrEmpty(String.Empty, "rationale"));
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Promise.NotNullOrWhiteSpace("value", "rationale");
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForNull()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(NULL_STRING, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForEmptyString()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale"));
        }

        #endregion
    }
}
