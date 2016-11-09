// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class PredicatesFacts
    {
        #region IsFlagsEnum()

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags",
            Justification = "[Intentionally] The rule does not apply here.")]
        public static void IsFlagsEnum_ReturnsFalse_ForNullInput()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            Assert.False(Predicates.IsFlagsEnum(type));
        }

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags",
            Justification = "[Intentionally] The rule does not apply here.")]
        public static void IsFlagsEnum_ReturnsTrue_ForFlagsEnumInput()
        {
            // Arrange
            var type = typeof(My.BitwiseEnum);

            // Act & Assert
            Assert.True(Predicates.IsFlagsEnum(type));
        }

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags",
            Justification = "[Intentionally] The rule does not apply here.")]
        public static void IsFlagsEnum_ReturnsFalse_ForNonFlagsEnumInput()
        {
            // Arrange
            var type = typeof(My.SimpleEnum);

            // Act & Assert
            Assert.False(Predicates.IsFlagsEnum(type));
        }

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags",
            Justification = "[Intentionally] The rule does not apply here.")]
        public static void IsFlagsEnum_ReturnsFalse_ForSimpleTypeInput()
        {
            // Arrange
            var type = typeof(Int32);

            // Act & Assert
            Assert.False(Predicates.IsFlagsEnum(type));
        }

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags",
            Justification = "[Intentionally] The rule does not apply here.")]
        public static void IsFlagsEnum_ReturnsFalse_ForNonEnumerationStructInput()
        {
            // Arrange
            var type = typeof(My.EmptyStruct);

            // Act & Assert
            Assert.False(Predicates.IsFlagsEnum(type));
        }

        #endregion

        #region IsEmptyOrWhiteSpace()

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForEmptyString()
        {
            // Act & Assert
            Assert.True(Predicates.IsEmptyOrWhiteSpace(String.Empty));
        }

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.True(Predicates.IsEmptyOrWhiteSpace(Constants.WhiteSpaceOnlyString));
        }

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNullString()
        {
            // Act & Assert
            Assert.False(Predicates.IsEmptyOrWhiteSpace(null));
        }

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNonEmptyOrWhiteSpaceString()
        {
            // Act & Assert
            Assert.False(Predicates.IsEmptyOrWhiteSpace("value"));
        }

        #endregion
    }

#endif
}
