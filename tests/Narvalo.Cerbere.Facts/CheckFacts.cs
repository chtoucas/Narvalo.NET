// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public sealed partial class CheckFacts : IClassFixture<DebugAssertFixture>
    {
        #region AssumeInvariant()

        [Fact]
        public static void AssumeInvariant_DoesNothing() => Check.AssumeInvariant(new Object());

        #endregion

        #region True()

        [Fact]
        public static void True_Passes_ForTrue() => Check.True(true);

        [ReleaseOnlyFact]
        public static void True_Passes_ForFalse() => Check.True(false);

        [DebugOnlyFact]
        public void True_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Check.True(false));

        #endregion

        #region False()

        [Fact]
        public static void False_Passes_ForFalse() => Check.False(false);

        [ReleaseOnlyFact]
        public static void False_Passes_ForTrue() => Check.False(true);

        [DebugOnlyFact]
        public void False_Fails_ForTrue()
            => Assert.Throws<DebugAssertFailedException>(() => Check.False(true));

        #endregion

        #region IsWhiteSpace()

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNull()
                => Assert.False(Check.IsWhiteSpace(null));

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForEmptyString()
            => Assert.False(Check.IsWhiteSpace(String.Empty));

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNonWhiteSpaceOnlyString()
            => Assert.False(Check.IsWhiteSpace("Whatever"));

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
            => Assert.True(Check.IsWhiteSpace(My.WhiteSpaceOnlyString));

        #endregion

        #region IsEmptyOrWhiteSpace()

        [Fact]
        public static void IsEmptyOrWhiteSpace_ReturnsTrue_ForEmptyString()
            => Assert.True(Check.IsEmptyOrWhiteSpace(String.Empty));

        [Fact]
        public static void IsEmptyOrWhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
            => Assert.True(Check.IsEmptyOrWhiteSpace(My.WhiteSpaceOnlyString));

        [Fact]
        public static void IsEmptyOrWhiteSpace_ReturnsFalse_ForNullString()
            => Assert.False(Check.IsEmptyOrWhiteSpace(null));

        [Fact]
        public static void IsEmptyOrWhiteSpace_ReturnsFalse_ForNonEmptyOrWhiteSpaceString()
        {
            Assert.False(Check.IsEmptyOrWhiteSpace("value"));
            Assert.False(Check.IsEmptyOrWhiteSpace("value with white spaces"));
        }

        #endregion

        #region IsFlagsEnum()

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNullInput()
        {
            Type type = null;

            Assert.False(Check.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsTrue_ForFlagsEnumInput()
        {
            var type = typeof(My.EnumBits);

            Assert.True(Check.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNonFlagsEnumInput()
        {
            var type = typeof(My.Enum012);

            Assert.False(Check.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForSimpleTypeInput()
        {
            var type = typeof(Int32);

            Assert.False(Check.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNonEnumerationStructInput()
        {
            var type = typeof(My.EmptyStruct);

            Assert.False(Check.IsFlagsEnum(type));
        }

        #endregion

        #region Unreachable()

        [Fact]
        public static void Unreachable_DoesNotThrow_ComprehensiveSwitch_1()
            => ComprehensiveSwitch1(My.Enum012.One);

        [Fact]
        public static void Unreachable_DoesNotThrow_ComprehensiveSwitch_2()
            => ComprehensiveSwitch2(My.Enum012.One);

        [Fact]
        public static void Unreachable_DoesNotThrow_ComprehensiveSwitch_3()
            => ComprehensiveSwitch3(My.Enum012.One);

        [Fact]
        public static void Unreachable_DoesNotThrow_IncompleteSwitch_1()
             => IncompleteSwitch1(My.Enum012.One);

        [Fact]
        public static void Unreachable_DoesNotThrow_IncompleteSwitch_2()
             => IncompleteSwitch2(My.Enum012.One);

        [Fact]
        public static void Unreachable_DoesNotThrow_IncompleteSwitch_3()
             => IncompleteSwitch3(My.Enum012.One);

        [Fact]
        public static void Unreachable_ThrowsControlFlowException_ForMissingValue_IncompleteSwitch_1()
             => Assert.Throws<ControlFlowException>(() => IncompleteSwitch1(My.Enum012.Two));

        [Fact]
        public static void Unreachable_ThrowsControlFlowException_ForMissingValue_IncompleteSwitch_2()
        {
            var ex = Record.Exception(() => IncompleteSwitch2(My.Enum012.Two));

            Assert.NotNull(ex);
            Assert.IsType<ControlFlowException>(ex);
            Assert.Equal(EXCEPTION_MESSAGE, ex.Message);
        }

        [Fact]
        public static void Unreachable_ThrowsCustomException_ForMissingValue_IncompleteSwitch_3()
             => Assert.Throws<InvalidOperationException>(() => IncompleteSwitch3(My.Enum012.Two));

        #endregion
    }

    // Helpers
    public partial class CheckFacts
    {
        private const string EXCEPTION_MESSAGE = "Found a missing case in the switch.";

        private static void ComprehensiveSwitch1(My.Enum012 value)
        {
            switch (value)
            {
                case My.Enum012.One:
                case My.Enum012.Two:
                case My.Enum012.Zero: break;
                default: throw Check.Unreachable();
            }
        }
        private static void ComprehensiveSwitch2(My.Enum012 value)
        {
            switch (value)
            {
                case My.Enum012.One:
                case My.Enum012.Two:
                case My.Enum012.Zero: break;
                default: throw Check.Unreachable(EXCEPTION_MESSAGE);
            }
        }
        private static void ComprehensiveSwitch3(My.Enum012 value)
        {
            switch (value)
            {
                case My.Enum012.One:
                case My.Enum012.Two:
                case My.Enum012.Zero: break;
                default: throw Check.Unreachable(new InvalidOperationException());
            }
        }

        private static void IncompleteSwitch1(My.Enum012 value)
        {
            switch (value)
            {
                case My.Enum012.One: break;
                default: throw Check.Unreachable();
            }
        }
        private static void IncompleteSwitch2(My.Enum012 value)
        {
            switch (value)
            {
                case My.Enum012.One: break;
                default: throw Check.Unreachable(EXCEPTION_MESSAGE);
            }
        }
        private static void IncompleteSwitch3(My.Enum012 value)
        {
            switch (value)
            {
                case My.Enum012.One: break;
                default: throw Check.Unreachable(new InvalidOperationException());
            }
        }
    }
}
