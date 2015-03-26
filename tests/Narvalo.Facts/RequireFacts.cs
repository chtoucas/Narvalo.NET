// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class RequireFacts
    {
        #region Object()

        [Fact]
        public static void Object_DoesNotThrow_ForNonNull()
        {
            // Act
            Require.Object("this");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void Object_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.Object((string)null));
        }

        #endregion

        #region Property()

        [Fact]
        public static void Property_DoesNotThrow_ForNonNull()
        {
            // Act
            Require.Property("value");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void Property_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.Property((string)null));
        }

        #endregion

        #region PropertyNotEmpty()

        [Fact]
        public static void PropertyNotEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Require.PropertyNotEmpty("value");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void PropertyNotEmpty_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotEmpty((string)null));
        }

        [Fact]
        public static void PropertyNotEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotEmpty(String.Empty));
        }

        #endregion

        #region PropertyNotWhiteSpace()

        [Fact]
        public static void PropertyNotWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Require.PropertyNotWhiteSpace("value");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotWhiteSpace((string)null));
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotWhiteSpace(String.Empty));
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotWhiteSpace("    "));
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull()
        {
            // Act
            Require.NotNull("value", "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void NotNull_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.NotNull((string)null, "parameter"));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Require.NotNullOrEmpty("value", "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrEmpty((string)null, "parameter"));
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrEmpty(String.Empty, "parameter"));
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Require.NotNullOrWhiteSpace("value", "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrWhiteSpace((string)null, "parameter"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrWhiteSpace(String.Empty, "parameter"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrWhiteSpace("    ", "parameter"));
        }

        #endregion

        #region InRange()

        [Fact]
        public static void InRange_DoesNotThrow_ForInt32InRange()
        {
            // Act
            Require.InRange(2, 1, 3, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInt64InRange()
        {
            // Act
            Require.InRange(2L, 1L, 3L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInt32LowerEnd()
        {
            // Act
            Require.InRange(1, 1, 3, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInt64LowerEnd()
        {
            // Act
            Require.InRange(1L, 1L, 3L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInt32UpperEnd()
        {
            // Act
            Require.InRange(2, 1, 3, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInt64UpperEnd()
        {
            // Act
            Require.InRange(2L, 1L, 3L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForInt32StrictlyBelowLowerEnd()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.InRange(0, 1, 3, "parameter"));
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForInt64StrictlyBelowLowerEnd()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.InRange(0L, 1L, 3L, "parameter"));
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForInt32StrictlyAboveUpperEnd()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.InRange(4, 1, 3, "parameter"));
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForInt64StrictlyAboveUpperEnd()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.InRange(4L, 1L, 3L, "parameter"));
        }

        #endregion

        #region GreaterThan()

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForInt32AboveMinValue()
        {
            // Act
            Require.GreaterThan(2, 1, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForInt64AboveMinValue()
        {
            // Act
            Require.GreaterThan(2L, 1L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt32MinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.GreaterThan(1, 1, "parameter"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt64MinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.GreaterThan(1L, 1L, "parameter"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt32BelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.GreaterThan(0, 1, "parameter"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt64BelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.GreaterThan(0L, 1L, "parameter"));
        }

        #endregion

        #region GreaterThanOrEqualTo()

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt32AboveMinValue()
        {
            // Act
            Require.GreaterThanOrEqualTo(2, 1, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt64AboveMinValue()
        {
            // Act
            Require.GreaterThanOrEqualTo(2L, 1L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt32MinValue()
        {
            // Act
            Require.GreaterThanOrEqualTo(1, 1, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt64MinValue()
        {
            // Act
            Require.GreaterThanOrEqualTo(1L, 1L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt32BelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.GreaterThanOrEqualTo(0, 1, "parameter"));
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt64BelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.GreaterThanOrEqualTo(0L, 1L, "parameter"));
        }

        #endregion

        #region LessThan()

        [Fact]
        public static void LessThan_DoesNotThrow_ForInt32BelowMinValue()
        {
            // Act
            Require.LessThan(0, 1, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void LessThan_DoesNotThrow_ForInt64BelowMinValue()
        {
            // Act
            Require.LessThan(0L, 1L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt32MinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.LessThan(1, 1, "parameter"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt64MinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.LessThan(1L, 1L, "parameter"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt32AboveMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.LessThan(2, 1, "parameter"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt64AboveMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.LessThan(2L, 1L, "parameter"));
        }

        #endregion

        #region LessThanOrEqualTo()

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt32BelowMinValue()
        {
            // Act
            Require.LessThanOrEqualTo(0, 1, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt64BelowMinValue()
        {
            // Act
            Require.LessThanOrEqualTo(0L, 1L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt32MinValue()
        {
            // Act
            Require.LessThanOrEqualTo(1, 1, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt64MinValue()
        {
            // Act
            Require.LessThanOrEqualTo(1L, 1L, "parameter");

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt32AboveMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.LessThanOrEqualTo(2, 1, "parameter"));
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt64AboveMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Require.LessThanOrEqualTo(2L, 1L, "parameter"));
        }

        #endregion
    }
}
