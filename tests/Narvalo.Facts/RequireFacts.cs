// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class RequireFacts
    {
        #region Object()

        [Fact]
        public static void Object_DoesNotThrow_ForNonNull()
        {
            // Act
            Require.Object("this");
        }

        [Fact]
        public static void Object_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.Object(My.NullString));
        }

        #endregion

        #region Property()

        [Fact]
        public static void Property_DoesNotThrow_ForNonNull()
        {
            // Act
            Require.Property("value");
        }

        [Fact]
        public static void Property_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.Property(My.NullString));
        }

        #endregion

        #region PropertyNotEmpty()

        [Fact]
        public static void PropertyNotEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Require.PropertyNotEmpty("value");
        }

        [Fact]
        public static void PropertyNotEmpty_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.PropertyNotEmpty(null));
        }

        [Fact]
        public static void PropertyNotEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotEmpty(String.Empty));
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNotThrow_ForNonNull()
        {
            // Act
            Require.NotNull("value", "parameter");
        }

        [Fact]
        public static void NotNull_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.NotNull(My.NullString, "parameter"));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNotThrow_ForNonNullOrEmptyString()
        {
            // Act
            Require.NotNullOrEmpty("value", "parameter");
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.NotNullOrEmpty(null, "parameter"));
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrEmpty(String.Empty, "parameter"));
        }

        #endregion
    }
}
