// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Cerbere
{
    using System;

    using Xunit;

    public static partial class GuardFacts
    {
        #region GreaterThan()

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForInputAboveMinValue()
        {
            Guard.GreaterThan(s_ValueAbove, s_MinValue, "paramName");
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInputBelowMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.GreaterThan(s_ValueBelow, s_MinValue, "paramName"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInputAtMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.GreaterThan(s_MinValue, s_MinValue, "paramName"));
        }

        #endregion

        #region GreaterThanOrEqualTo()

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInputAtMinValue()
        {
            Guard.GreaterThanOrEqualTo(s_MinValue, s_MinValue, "paramName");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInputAboveMinValue()
        {
            Guard.GreaterThanOrEqualTo(s_ValueAbove, s_MinValue, "paramName");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInputBelowMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.GreaterThanOrEqualTo(s_ValueBelow, s_MinValue, "paramName"));
        }

        #endregion

        #region LessThan()

        [Fact]
        public static void LessThan_DoesNotThrow_ForInputBelowMaxValue()
        {
            Guard.LessThan(s_ValueBelow, s_MaxValue, "paramName");
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInputAtMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.LessThan(s_MaxValue, s_MaxValue, "paramName"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInputAboveMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.LessThan(s_ValueAbove, s_MaxValue, "paramName"));
        }

        #endregion

        #region LessThanOrEqualTo()

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInputBelowMaxValue()
        {
            Guard.LessThanOrEqualTo(s_ValueBelow, s_MaxValue, "paramName");
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInputAtMaxValue()
        {
            Guard.LessThanOrEqualTo(s_MaxValue, s_MaxValue, "paramName");
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInputAboveMaxValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.LessThanOrEqualTo(s_ValueAbove, s_MaxValue, "paramName"));
        }

        #endregion
    }

    // Primitive numeric types.
    public static partial class GuardFacts
    {
        #region Range()

        [Fact]
        public static void Range_DoesNotThrow_ForInt32InRange()
        {
            Guard.Range(2, 1, 3, "paramName");
        }

        [Fact]
        public static void Range_DoesNotThrow_ForInt64InRange()
        {
            Guard.Range(2L, 1L, 3L, "paramName");
        }

        [Fact]
        public static void Range_DoesNotThrow_ForInt32AtLowerEnd()
        {
            Guard.Range(1, 1, 3, "paramName");
        }

        [Fact]
        public static void Range_DoesNotThrow_ForInt64AtLowerEnd()
        {
            Guard.Range(1L, 1L, 3L, "paramName");
        }

        [Fact]
        public static void Range_DoesNotThrow_ForInt32AtUpperEnd()
        {
            Guard.Range(2, 1, 3, "paramName");
        }

        [Fact]
        public static void Range_DoesNotThrow_ForInt64AtUpperEnd()
        {
            Guard.Range(2L, 1L, 3L, "paramName");
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForInvalidInt32Range()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Range(0, 3, 1, "paramName"));
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForInvalidInt64Range()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Range(0L, 3L, 1L, "paramName"));
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForInt32BelowLowerEnd()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Range(0, 1, 3, "paramName"));
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForInt64BelowLowerEnd()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Range(0L, 1L, 3L, "paramName"));
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForInt32AboveUpperEnd()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Range(4, 1, 3, "paramName"));
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForInt64AboveUpperEnd()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Range(4L, 1L, 3L, "paramName"));
        }

        #endregion

        #region GreaterThan()

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForInt32AboveMinValue()
        {
            Guard.GreaterThan(2, 1, "paramName");
        }

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForInt64AboveMinValue()
        {
            Guard.GreaterThan(2L, 1L, "paramName");
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt32AtMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThan(1, 1, "paramName"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt64AtMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThan(1L, 1L, "paramName"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt32BelowMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThan(0, 1, "paramName"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInt64BelowMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThan(0L, 1L, "paramName"));
        }

        #endregion

        #region GreaterThanOrEqualTo()

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt32AboveMinValue()
        {
            Guard.GreaterThanOrEqualTo(2, 1, "paramName");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt64AboveMinValue()
        {
            Guard.GreaterThanOrEqualTo(2L, 1L, "paramName");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt32AtMinValue()
        {
            Guard.GreaterThanOrEqualTo(1, 1, "paramName");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInt64AtMinValue()
        {
            Guard.GreaterThanOrEqualTo(1L, 1L, "paramName");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt32BelowMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThanOrEqualTo(0, 1, "paramName"));
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt64BelowMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.GreaterThanOrEqualTo(0L, 1L, "paramName"));
        }

        #endregion

        #region LessThan()

        [Fact]
        public static void LessThan_DoesNotThrow_ForInt32BelowMinValue()
        {
            Guard.LessThan(0, 1, "paramName");
        }

        [Fact]
        public static void LessThan_DoesNotThrow_ForInt64BelowMinValue()
        {
            Guard.LessThan(0L, 1L, "paramName");
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt32AtMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThan(1, 1, "paramName"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt64AtMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThan(1L, 1L, "paramName"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt32AboveMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThan(2, 1, "paramName"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInt64AboveMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThan(2L, 1L, "paramName"));
        }

        #endregion

        #region LessThanOrEqualTo()

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt32BelowMinValue()
        {
            Guard.LessThanOrEqualTo(0, 1, "paramName");
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt64BelowMinValue()
        {
            Guard.LessThanOrEqualTo(0L, 1L, "paramName");
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt32AtMinValue()
        {
            Guard.LessThanOrEqualTo(1, 1, "paramName");
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInt64AtMinValue()
        {
            Guard.LessThanOrEqualTo(1L, 1L, "paramName");
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt32AboveMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThanOrEqualTo(2, 1, "paramName"));
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInt64AboveMinValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.LessThanOrEqualTo(2L, 1L, "paramName"));
        }

        #endregion
    }

    // Helpers.
    public static partial class GuardFacts
    {
        private static readonly My.ComparableStruct s_ValueBelow = new My.ComparableStruct(0);
        private static readonly My.ComparableStruct s_MinValue = new My.ComparableStruct(1);
        private static readonly My.ComparableStruct s_ValueInRange = new My.ComparableStruct(2);
        private static readonly My.ComparableStruct s_MaxValue = new My.ComparableStruct(3);
        private static readonly My.ComparableStruct s_ValueAbove = new My.ComparableStruct(4);
    }
}
