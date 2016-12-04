// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class EnforceFacts
    {
        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Enforce.NotNullOrWhiteSpace("value", "parameter");
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Enforce.NotNullOrWhiteSpace(null, "parameter"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Enforce.NotNullOrWhiteSpace(String.Empty, "parameter"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => Enforce.NotNullOrWhiteSpace(My.WhiteSpaceOnlyString, "parameter"));
        }

        #endregion

        #region PropertyNotWhiteSpace()

        [Fact]
        public static void PropertyNotWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Enforce.PropertyNotWhiteSpace("value");
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Enforce.PropertyNotWhiteSpace(null));
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Enforce.PropertyNotWhiteSpace(String.Empty));
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Enforce.PropertyNotWhiteSpace(My.WhiteSpaceOnlyString));
        }

        #endregion

    }
}
