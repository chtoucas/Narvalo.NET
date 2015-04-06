// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class CheckFacts
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

        [Fact]
        public static void Condition_Throws_ForFalseCondition()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.Condition(false, "rationale"));
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull()
        {
            // Act
            Check.NotNull(String.Empty, "rationale");
        }

        [Fact]
        public static void NotNull_Throws_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.NotNull(NULL_STRING, "rationale"));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Check.NotNullOrEmpty("value", "rationale");
        }

        [Fact]
        public static void NotNullOrEmpty_Throws_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.NotNullOrEmpty(NULL_STRING, "rationale"));
        }

        [Fact]
        public static void NotNullOrEmpty_Throws_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.NotNullOrEmpty(String.Empty, "rationale"));
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Check.NotNullOrWhiteSpace("value", "rationale");
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.NotNullOrWhiteSpace(NULL_STRING, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Check.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale"));
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class CheckFacts
    {
        #region Condition()

        [Fact]
        public static void Condition_ThrowsIllegalConditionException_ForFalseCondition()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.Condition(false, "rationale"));
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_ThrowsIllegalConditionException_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.NotNull(NULL_STRING, "rationale"));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_ThrowsIllegalConditionException_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.NotNullOrEmpty(NULL_STRING, "rationale"));
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsIllegalConditionException_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.NotNullOrEmpty(String.Empty, "rationale"));
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsIllegalConditionException_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.NotNullOrWhiteSpace(NULL_STRING, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsIllegalConditionException_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsIllegalConditionException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Check.NotNullOrWhiteSpace(WHITESPACE_ONLY_STRING, "rationale"));
        }

        #endregion
    }

#endif
}
