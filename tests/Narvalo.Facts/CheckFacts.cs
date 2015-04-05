// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

    // NB: This is one of those exceptional cases where we use different sets of tests 
    // whether we build the tests in debug or in release configuration.
    public static class CheckFacts
    {
        private const string NULL_STRING = null;
        private const string WHITESPACE_ONLY_STRING = "     ";

        #region Condition()

        [Fact]
        public static void Condition_DoesNotThrow_ForTrueCondition()
        {
            // Act
            Check.Condition(true, "rationale");
        }

        [DebugOnlyFact]
        public static void Condition_Throws_ForFalseCondition_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.Condition(false, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void Condition_DoesNotThrow_ForFalseCondition_InReleaseBuild()
        {
            // Act
            Check.Condition(false, "rationale");
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull()
        {
            // Act
            Check.NotNull(String.Empty, "rationale");
        }

        [DebugOnlyFact]
        public static void NotNull_Throws_ForNull_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.NotNull(NULL_STRING, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void NotNull_DoesNotThrow_ForNull_InReleaseBuild()
        {
            // Act
            Check.NotNull(NULL_STRING, "rationale");
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Check.NotNullOrEmpty("value", "rationale");
        }

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Throws_ForNull_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.NotNullOrEmpty(NULL_STRING, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNull_InReleaseBuild()
        {
            // Act
            Check.NotNullOrEmpty(NULL_STRING, "rationale");
        }

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Throws_ForEmptyString_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.NotNullOrEmpty(String.Empty, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_DoesNotThrow_ForEmptyString_InReleaseBuild()
        {
            // Act
            Check.NotNullOrEmpty(String.Empty, "rationale");
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Check.NotNullOrWhiteSpace("value", "rationale");
        }

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Throws_ForNull_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.NotNullOrWhiteSpace(NULL_STRING, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNull_InReleaseBuild()
        {
            // Act
            Check.NotNullOrWhiteSpace(NULL_STRING, "rationale");
        }

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Throws_ForEmptyString_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForEmptyString_InReleaseBuild()
        {
            // Act
            Check.NotNullOrWhiteSpace(String.Empty, "rationale");
        }

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Throws_ForWhiteSpaceOnlyString_InDebugBuild()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Check.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale"));
        }

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForWhiteSpaceOnlyString_InReleaseBuild()
        {
            // Act
            Check.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale");
        }

        #endregion
    }
}
