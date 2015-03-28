// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    // NB: This is one of those exceptional cases where we use different sets of tests 
    // whether we build the tests in debug or in release configuration.
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

#if DEBUG
        [Fact]
#endif
        public static void Condition_Throws_ForFalseCondition_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.Condition(false, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void Condition_DoesNotThrow_ForFalseCondition_InReleaseBuild()
        {
            // Act
            Promise.Condition(false, "rationale");
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull()
        {
            // Act
            Promise.NotNull(String.Empty, "rationale");
        }

#if DEBUG
        [Fact]
#endif
        public static void NotNull_Throws_ForNull_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNull(NULL_STRING, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void NotNull_DoesNotThrow_ForNull_InReleaseBuild()
        {
            // Act
            Promise.NotNull(NULL_STRING, "rationale");
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Promise.NotNullOrEmpty("value", "rationale");
        }

#if DEBUG
        [Fact]
#endif
        public static void NotNullOrEmpty_Throws_ForNull_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrEmpty(NULL_STRING, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void NotNullOrEmpty_DoesNotThrow_ForNull_InReleaseBuild()
        {
            // Act
            Promise.NotNullOrEmpty(NULL_STRING, "rationale");
        }

#if DEBUG
        [Fact]
#endif
        public static void NotNullOrEmpty_Throws_ForEmptyString_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrEmpty(String.Empty, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void NotNullOrEmpty_DoesNotThrow_ForEmptyString_InReleaseBuild()
        {
            // Act
            Promise.NotNullOrEmpty(String.Empty, "rationale");
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Promise.NotNullOrWhiteSpace("value", "rationale");
        }

#if DEBUG
        [Fact]
#endif
        public static void NotNullOrWhiteSpace_Throws_ForNull_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(NULL_STRING, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNull_InReleaseBuild()
        {
            // Act
            Promise.NotNullOrWhiteSpace(NULL_STRING, "rationale");
        }

#if DEBUG
        [Fact]
#endif
        public static void NotNullOrWhiteSpace_Throws_ForEmptyString_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForEmptyString_InReleaseBuild()
        {
            // Act
            Promise.NotNullOrWhiteSpace(String.Empty, "rationale");
        }

#if DEBUG
        [Fact]
#endif
        public static void NotNullOrWhiteSpace_Throws_ForWhiteSpaceOnlyString_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale"));
        }

#if !DEBUG
        [Fact]
#endif
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForWhiteSpaceOnlyString_InReleaseBuild()
        {
            // Act
            Promise.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale");
        }

        #endregion
    }
}
