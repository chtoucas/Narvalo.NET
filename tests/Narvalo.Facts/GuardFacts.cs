// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class GuardFacts
    {
        private static readonly My.ComparableStruct s_ValueBelow = new My.ComparableStruct(0);
        private static readonly My.ComparableStruct s_MinValue = new My.ComparableStruct(1);
        private static readonly My.ComparableStruct s_ValueInRange = new My.ComparableStruct(2);
        private static readonly My.ComparableStruct s_MaxValue = new My.ComparableStruct(3);
        private static readonly My.ComparableStruct s_ValueAbove = new My.ComparableStruct(4);
    }

    public static partial class GuardFacts
    {
        #region InRange()

        [Fact]
        public static void InRange_DoesNotThrow_ForInputAtMinValue()
        {
            // Act
            Guard.InRange(s_MinValue, s_MinValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInputInRange()
        {
            // Act
            Guard.InRange(s_ValueInRange, s_MinValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForInputAtMaxValue()
        {
            // Act
            Guard.InRange(s_MinValue, s_MinValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void InRange_ThrowsArgumentException_ForInvalidRange()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.InRange(s_ValueBelow, s_MaxValue, s_MinValue, "parameter"));
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForInputBelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.InRange(s_ValueBelow, s_MinValue, s_MaxValue, "parameter"));
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForInputAboveMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.InRange(s_ValueAbove, s_MinValue, s_MaxValue, "parameter"));
        }

        #endregion

        #region GreaterThan()

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForInputAboveMinValue()
        {
            // Act
            Guard.GreaterThan(s_ValueAbove, s_MinValue, "parameter");
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInputBelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.GreaterThan(s_ValueBelow, s_MinValue, "parameter"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForInputAtMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.GreaterThan(s_MinValue, s_MinValue, "parameter"));
        }

        #endregion

        #region GreaterThanOrEqualTo()

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInputAtMinValue()
        {
            // Act
            Guard.GreaterThanOrEqualTo(s_MinValue, s_MinValue, "parameter");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForInputAboveMinValue()
        {
            // Act
            Guard.GreaterThanOrEqualTo(s_ValueAbove, s_MinValue, "parameter");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInputBelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.GreaterThanOrEqualTo(s_ValueBelow, s_MinValue, "parameter"));
        }

        #endregion

        #region LessThan()

        [Fact]
        public static void LessThan_DoesNotThrow_ForInputBelowMaxValue()
        {
            // Act
            Guard.LessThan(s_ValueBelow, s_MaxValue, "parameter");
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInputAtMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.LessThan(s_MaxValue, s_MaxValue, "parameter"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForInputAboveMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.LessThan(s_ValueAbove, s_MaxValue, "parameter"));
        }

        #endregion

        #region LessThanOrEqualTo()

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInputBelowMaxValue()
        {
            // Act
            Guard.LessThanOrEqualTo(s_ValueBelow, s_MaxValue, "parameter");
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForInputAtMaxValue()
        {
            // Act
            Guard.LessThanOrEqualTo(s_MaxValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForInputAboveMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Guard.LessThanOrEqualTo(s_ValueAbove, s_MaxValue, "parameter"));
        }

        #endregion
    }
}
