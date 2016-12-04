// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    // TODO: Add tests for debug configuration.
    // See http://stackoverflow.com/questions/9230065/how-to-ensure-a-debug-assert-fires-correctly-using-nunit.
    public static partial class CheckFacts
    {
        #region True()

        [Fact]
        public static void True_DoesNothing_ForTrue()
        {
            Check.True(true);
        }

        [Fact]
        public static void True_DoesNothing_ForFalse_NonDebugConfiguration()
        {
#if !DEBUG
            Check.True(false);
#endif
        }

        #endregion

        #region False()

        [Fact]
        public static void False_DoesNothing_ForFalse()
        {
            Check.False(false);
        }

        [Fact]
        public static void False_DoesNothing_ForTrue_NonDebugConfiguration()
        {
#if !DEBUG
            Check.False(true);
#endif
        }

        #endregion

        #region AssumeInvariant()

        [Fact]
        public static void AssumeInvariant_DoesNothing()
        {
            Check.AssumeInvariant(new Object());
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
            Assert.Equal(ExceptionMessage, ex.Message);
        }

        [Fact]
        public static void Unreachable_ThrowsCustomException_ForMissingValue_IncompleteSwitch_3()
             => Assert.Throws<InvalidOperationException>(() => IncompleteSwitch3(My.Enum012.Two));

        #endregion
    }

    // Helpers
    public static partial class CheckFacts
    {
        private const string ExceptionMessage = "Found a missing case in the switch.";

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
                default: throw Check.Unreachable(ExceptionMessage);
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
                default: throw Check.Unreachable(ExceptionMessage);
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
