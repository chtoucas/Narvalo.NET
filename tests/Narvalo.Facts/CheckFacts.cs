// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class CheckFacts
    {
        #region AssumeInvariant()

        [Fact]
        public static void Invariant_DoesNothing()
        {
            // Arrange a Act
            Check.AssumeInvariant(new Object());
        }

        #endregion

        #region Unreachable()

        [Fact]
        public static void Unreachable_DoesNotThrow_ForComprehensiveSwitch()
        {
            // Act
            ComprehensiveSwitch(MyEnum_.One);
        }

        [Fact]
        public static void Unreachable_ThrowsInvalidOperationException_ForIncompleteSwitch()
        {
            // Act & Assert
            Assert.Throws<NotSupportedException>(() => IncompleteSwitch(MyEnum_.Two));
        }

        [Fact]
        public static void Unreachable_ThrowsCustomException_ForIncompleteSwitch()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => IncompleteSwitchWithCustomException(MyEnum_.Two));
        }

        #endregion
    }

    public static partial class CheckFacts
    {
        private enum MyEnum_
        {
            One,
            Two,
        }

        private static string ComprehensiveSwitch(MyEnum_ value)
        {
            switch (value)
            {
                case MyEnum_.One:
                case MyEnum_.Two:
                    return "OK";

                default:
                    throw Check.Unreachable("Found a missing case in the switch.");
            }
        }

        private static string IncompleteSwitch(MyEnum_ value)
        {
            switch (value)
            {
                case MyEnum_.One:
                    return "OK";

                default:
                    throw Check.Unreachable("Found a missing case in the switch.");
            }
        }

        private static string IncompleteSwitchWithCustomException(MyEnum_ value)
        {
            switch (value)
            {
                case MyEnum_.One:
                    return "OK";

                default:
                    throw Check.Unreachable(
                        new InvalidOperationException("Found a missing case in the switch."));
            }
        }
    }
}
