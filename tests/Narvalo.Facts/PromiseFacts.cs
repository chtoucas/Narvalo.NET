// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class PromiseFacts
    {
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
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Promise.Condition(false, "rationale"));
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
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Promise.NotNull(Constants.NullString, "rationale"));
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
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Promise.NotNullOrEmpty(Constants.NullString, "rationale"));
        }

        [Fact]
        public static void NotNullOrEmpty_Throws_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Promise.NotNullOrEmpty(String.Empty, "rationale"));
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
            AssertExt.DebugOnly.ThrowsAny<Exception>(
                () => Promise.NotNullOrWhiteSpace(Constants.NullString, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(() => Promise.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_Throws_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.ThrowsAny<Exception>(
                () => Promise.NotNullOrWhiteSpace(Constants.WhiteSpaceOnlyString, "rationale"));
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class PromiseFacts
    {
        #region Condition()

        [Fact]
        public static void Condition_ThrowsIllegalConditionException_ForFalseCondition()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(() => Promise.Condition(false, "rationale"));
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_ThrowsIllegalConditionException_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(
                () => Promise.NotNull(Constants.NullString, "rationale"));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_ThrowsIllegalConditionException_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(
                () => Promise.NotNullOrEmpty(Constants.NullString, "rationale"));
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsIllegalConditionException_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(
                () => Promise.NotNullOrEmpty(String.Empty, "rationale"));
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsIllegalConditionException_ForNull()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(
                () => Promise.NotNullOrWhiteSpace(Constants.NullString, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsIllegalConditionException_ForEmptyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(
                () => Promise.NotNullOrWhiteSpace(String.Empty, "rationale"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsIllegalConditionException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            AssertExt.DebugOnly.Throws<Internal.IllegalConditionException>(
                () => Promise.NotNullOrWhiteSpace(Constants.WhiteSpaceOnlyString, "rationale"));
        }

        #endregion
    }

#endif
}
