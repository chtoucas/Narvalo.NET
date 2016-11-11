// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
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
            Assert.Throws<ArgumentNullException>(() => Require.Object(Constants.NullString));
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
            Assert.Throws<ArgumentNullException>(() => Require.Property(Constants.NullString));
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
            Assert.Throws<ArgumentNullException>(() => Require.PropertyNotEmpty(Constants.NullString));
        }

        [Fact]
        public static void PropertyNotEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotEmpty(String.Empty));
        }

        #endregion

        #region PropertyNotWhiteSpace()

        [Fact]
        public static void PropertyNotWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Require.PropertyNotWhiteSpace("value");
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.PropertyNotWhiteSpace(Constants.NullString));
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotWhiteSpace(String.Empty));
        }

        [Fact]
        public static void PropertyNotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.PropertyNotWhiteSpace(Constants.WhiteSpaceOnlyString));
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
            Assert.Throws<ArgumentNullException>(() => Require.NotNull(Constants.NullString, "parameter"));
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
            Assert.Throws<ArgumentNullException>(() => Require.NotNullOrEmpty(Constants.NullString, "parameter"));
        }

        [Fact]
        public static void NotNullOrEmpty_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrEmpty(String.Empty, "parameter"));
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_DoesNotThrow_ForNonNullOrWhiteSpaceString()
        {
            // Act
            Require.NotNullOrWhiteSpace("value", "parameter");
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentNullException_ForNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Require.NotNullOrWhiteSpace(Constants.NullString, "parameter"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Require.NotNullOrWhiteSpace(String.Empty, "parameter"));
        }

        [Fact]
        public static void NotNullOrWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => Require.NotNullOrWhiteSpace(Constants.WhiteSpaceOnlyString, "parameter"));
        }

        #endregion
    }
}
