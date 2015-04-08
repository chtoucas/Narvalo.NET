// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class AcknowledgeFacts
    {
        #region Object()

        [Fact]
        public static void Object_DoesNotThrow_ForNull()
        {
            // Act
            Acknowledge.Object((string)null);
        }

        [Fact]
        public static void Object_DoesNotThrow_ForNonNull()
        {
            // Act
            Acknowledge.Object("this");
        }

        #endregion

        #region Unreachable()

        [Fact]
        public static void Unreachable_DoesNotThrow_ForComprehensiveSwitch()
        {
            // Act
            ComprehensiveSwitch_(MyEnum_.One);
        }

        [Fact]
        public static void Unreachable_ThrowsInvalidOperationException_ForIncompleteSwitch()
        {
            // Act & Assert
            Assert.Throws<NotSupportedException>(() => IncompleteSwitch_(MyEnum_.Two));
        }

        [Fact]
        public static void Unreachable_ThrowsCustomException_ForIncompleteSwitch()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => IncompleteSwitchWithCustomException_(MyEnum_.Two));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNothing()
        {
            // Act
            Acknowledge.NotNullOrEmpty(null);
            Acknowledge.NotNullOrEmpty(String.Empty);
            Acknowledge.NotNullOrEmpty("value");
        }

        #endregion

        #region Object()

        [Fact]
        public static void Object_DoesNothing()
        {
            // Act
            Acknowledge.Object((string)null);
            Acknowledge.Object("value");
        }

        #endregion
    }

    public static partial class AcknowledgeFacts
    {
        private enum MyEnum_
        {
            One,
            Two,
        }

        private static string ComprehensiveSwitch_(MyEnum_ value)
        {
            switch (value)
            {
                case MyEnum_.One:
                case MyEnum_.Two:
                    return "OK";

                default:
                    throw Acknowledge.Unreachable("Found a missing case in the switch.");
            }
        }

        private static string IncompleteSwitch_(MyEnum_ value)
        {
            switch (value)
            {
                case MyEnum_.One:
                    return "OK";

                default:
                    throw Acknowledge.Unreachable("Found a missing case in the switch.");
            }
        }

        private static string IncompleteSwitchWithCustomException_(MyEnum_ value)
        {
            switch (value)
            {
                case MyEnum_.One:
                    return "OK";

                default:
                    throw Acknowledge.Unreachable(
                        new InvalidOperationException("Found a missing case in the switch."));
            }
        }
    }
}
