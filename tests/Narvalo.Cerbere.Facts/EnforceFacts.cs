// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class EnforceFacts
    {
        #region State()

        [Fact]
        public static void State_DoesNotThrow_ForTrue_1() => Enforce.State(true);

        [Fact]
        public static void State_DoesNotThrow_ForTrue_2() => Enforce.State(true, "My message");

        [Fact]
        public static void State_ThrowsInvalidOperationException_ForFalse_1()
            => Assert.Throws<InvalidOperationException>(() => Enforce.State(false));

        [Fact]
        public static void State_ThrowsInvalidOperationException_ForFalse_2()
        {
            // Arrange
            var message = "My message";
            Action act = () => Enforce.State(false, message);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal(message, ex.Message);
        }

        #endregion

        #region True()

        [Fact]
        public static void True_DoesNotThrow_ForTrue_1() => Enforce.True(true, "paramName");

        [Fact]
        public static void True_DoesNotThrow_ForTrue_2() => Enforce.True(true, "paramName", "My message");

        [Fact]
        public static void True_ThrowsArgumentException_ForFalse_1()
        {
            // Arrange
            var paramName = "paramName";
            Action act = () => Enforce.True(false, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void True_ThrowsArgumentException_ForFalse_2()
        {
            // Arrange
            var paramName = "paramName";
            var message = "My message";
            Action act = () => Enforce.True(false, paramName, message);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
            // ArgumentException appends some info to our message.
            Assert.StartsWith(message, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Range()

        [Fact]
        public static void Range_DoesNotThrow_ForTrue_1() => Enforce.Range(true, "paramName");

        [Fact]
        public static void Range_DoesNotThrow_ForTrue_2() => Enforce.Range(true, "paramName", "My message");

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForFalse_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Enforce.Range(false, "paramName"));
            // Arrange
            var paramName = "paramName";
            Action act = () => Enforce.Range(false, paramName);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForFalse_2()
        {
            // Arrange
            var paramName = "paramName";
            var message = "My message";
            Action act = () => Enforce.Range(false, paramName, message);

            // Act
            var ex = Record.Exception(act);

            // Assert
            Assert.NotNull(ex);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
            // NB: ArgumentOutOfRangeException appends some info to our message.
            Assert.StartsWith(message, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Range<T>()

        [Theory]
        [MemberData(nameof(InRangeValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Range_DoesNotThrow_ForInRangeValue(My.ComparableStruct value)
            => Enforce.Range(value, s_MinValue, s_MaxValue, "paramName");

        [Theory]
        [MemberData(nameof(OutOfRangeValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Range_ThrowsArgumentOutOfRangeException_ForOutOfRangeValue(My.ComparableStruct value)
            => Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.Range(value, s_MinValue, s_MaxValue, "paramName"));

        [Theory]
        [MemberData(nameof(RangeValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Range_ThrowsArgumentOutOfRangeException_InvalidRange(My.ComparableStruct value)
            => Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.Range(value, s_MaxValue, s_MinValue, "paramName"));

        [Fact]
        public static void Range_DoesNotThrow_ForInRangeValue_DegenerateRange()
            => Enforce.Range(s_MinValue, s_MinValue, s_MinValue, "paramName");

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForOutOfRangeValue_DegenerateRange_1()
            => Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.Range(s_ValueBelow, s_MinValue, s_MinValue, "paramName"));

        [Fact]
        public static void Range_ThrowsArgumentOutOfRangeException_ForOutOfRangeValue_DegenerateRange_2()
            => Assert.Throws<ArgumentOutOfRangeException>(
                () => Enforce.Range(s_ValueAbove, s_MinValue, s_MinValue, "paramName"));

        #endregion

        #region NotWhiteSpace()

        [Fact]
        public static void NotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
            => Assert.Throws<ArgumentException>(() => Enforce.NotWhiteSpace(My.WhiteSpaceOnlyString, "paramName"));

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForNull()
            => Enforce.NotWhiteSpace(null, "paramName");

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForEmptyString()
            => Enforce.NotWhiteSpace(String.Empty, "paramName");

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForNonWhiteSpaceString()
            => Enforce.NotWhiteSpace("Whatever", "paramName");

        #endregion
    }

    public static partial class EnforceFacts
    {
        private static readonly My.ComparableStruct s_ValueBelow = new My.ComparableStruct(0);
        private static readonly My.ComparableStruct s_MinValue = new My.ComparableStruct(1);
        private static readonly My.ComparableStruct s_ValueInRange = new My.ComparableStruct(2);
        private static readonly My.ComparableStruct s_MaxValue = new My.ComparableStruct(3);
        private static readonly My.ComparableStruct s_ValueAbove = new My.ComparableStruct(4);

        public static IEnumerable<object[]> RangeValues
        {
            get
            {
                yield return new object[] { s_ValueBelow };
                yield return new object[] { s_MinValue };
                yield return new object[] { s_ValueInRange };
                yield return new object[] { s_MaxValue };
                yield return new object[] { s_ValueAbove };
            }
        }

        public static IEnumerable<object[]> OutOfRangeValues
        {
            get
            {
                yield return new object[] { s_ValueBelow };
                yield return new object[] { s_ValueAbove };
            }
        }

        public static IEnumerable<object[]> InRangeValues
        {
            get
            {
                yield return new object[] { s_MinValue };
                yield return new object[] { s_ValueInRange };
                yield return new object[] { s_MaxValue };
            }
        }
    }
}
