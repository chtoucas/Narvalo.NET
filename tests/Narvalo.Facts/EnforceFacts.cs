// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class EnforceFacts
    {
        private static readonly Stub_ s_ValueBelow = new Stub_(0);
        private static readonly Stub_ s_MinValue = new Stub_(1);
        private static readonly Stub_ s_ValueInRange = new Stub_(2);
        private static readonly Stub_ s_MaxValue = new Stub_(3);
        private static readonly Stub_ s_ValueAbove = new Stub_(4);

        #region InRange()

        [Fact]
        public static void InRange_DoesNotThrow_ForValueAtMinValue()
        {
            // Act
            Enforce.InRange(s_MinValue, s_MinValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForValueInRange()
        {
            // Act
            Enforce.InRange(s_ValueInRange, s_MinValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void InRange_DoesNotThrow_ForValueAtMaxValue()
        {
            // Act
            Enforce.InRange(s_MinValue, s_MinValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForValueBelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.InRange(s_ValueBelow, s_MinValue, s_MaxValue, "parameter"));
        }

        [Fact]
        public static void InRange_ThrowsArgumentOutOfRangeException_ForValueAboveMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.InRange(s_ValueAbove, s_MinValue, s_MaxValue, "parameter"));
        }

        #endregion

        #region GreaterThan

        [Fact]
        public static void GreaterThan_DoesNotThrow_ForValueAboveMinValue()
        {
            // Act
            Enforce.GreaterThan(s_ValueAbove, s_MinValue, "parameter");
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForValueBelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.GreaterThan(s_ValueBelow, s_MinValue, "parameter"));
        }

        [Fact]
        public static void GreaterThan_ThrowsArgumentOutOfRangeException_ForValueAtMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.GreaterThan(s_MinValue, s_MinValue, "parameter"));
        }

        #endregion

        #region GreaterThanOrEqualTo

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForValueAtMinValue()
        {
            // Act
            Enforce.GreaterThanOrEqualTo(s_MinValue, s_MinValue, "parameter");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_DoesNotThrow_ForValueAboveMinValue()
        {
            // Act
            Enforce.GreaterThanOrEqualTo(s_ValueAbove, s_MinValue, "parameter");
        }

        [Fact]
        public static void GreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForValueBelowMinValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.GreaterThanOrEqualTo(s_ValueBelow, s_MinValue, "parameter"));
        }

        #endregion

        #region LessThan

        [Fact]
        public static void LessThan_DoesNotThrow_ForValueBelowMaxValue()
        {
            // Act
            Enforce.LessThan(s_ValueBelow, s_MaxValue, "parameter");
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForValueAtMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.LessThan(s_MaxValue, s_MaxValue, "parameter"));
        }

        [Fact]
        public static void LessThan_ThrowsArgumentOutOfRangeException_ForValueAboveMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.LessThan(s_ValueAbove, s_MaxValue, "parameter"));
        }

        #endregion

        #region LessThanOrEqualTo

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForValueBelowMaxValue()
        {
            // Act
            Enforce.LessThanOrEqualTo(s_ValueBelow, s_MaxValue, "parameter");
        }

        [Fact]
        public static void LessThanOrEqualTo_DoesNotThrow_ForValueAtMaxValue()
        {
            // Act
            Enforce.LessThanOrEqualTo(s_MaxValue, s_MaxValue, "parameter");
        }

        [Fact]
        public static void LessThanOrEqualTo_ThrowsArgumentOutOfRangeException_ForValueAboveMaxValue()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.LessThanOrEqualTo(s_ValueAbove, s_MaxValue, "parameter"));
        }

        #endregion

        #region Stubs

        private struct Stub_ : IComparable<Stub_>
        {
            private readonly int _value;

            public Stub_(int value)
            {
                _value = value;
            }

            public int CompareTo(Stub_ other)
            {
                return _value.CompareTo(other._value);
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }

        #endregion
    }
}
