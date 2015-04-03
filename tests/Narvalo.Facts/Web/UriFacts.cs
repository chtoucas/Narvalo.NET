// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;

    using Xunit;

    public static class UriFacts
    {
        #region ToProtocolRelativeString()

        [Fact]
        public static void ToProtocolRelativeString_ThrowsArgumentNullException_ForNullObject()
        {
            // Arrange
            Uri uri = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => uri.ToProtocolRelativeString());
        }

        [Fact]
        public static void ToProtocolRelativeString_ThrowsNotSupportedException_ForUnsupportedScheme()
        {
            // Arrange
            Uri uri = new Uri("mailto:nobody@nowhere.org");

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => uri.ToProtocolRelativeString());
        }

        [Fact]
        public static void Link_ReturnsExpectedString_ForAbsoluteUriAndHttpScheme()
        {
            // Arrange
            Uri uri = new Uri("http://localhost/my/relative/path");

            // Act
            var result = uri.ToProtocolRelativeString();

            // Assert
            Assert.Equal("//localhost/my/relative/path", result);
        }

        [Fact]
        public static void Link_ReturnsExpectedString_ForAbsoluteUriAndHttpsScheme()
        {
            // Arrange
            Uri uri = new Uri("https://localhost/my/relative/path");

            // Act
            var result = uri.ToProtocolRelativeString();

            // Assert
            Assert.Equal("//localhost/my/relative/path", result);
        }

        [Fact]
        public static void Link_ReturnsExpectedString_ForRelativeUri()
        {
            // Arrange
            Uri uri = new Uri("/my/relative/path", UriKind.Relative);

            // Act
            var result = uri.ToProtocolRelativeString();

            // Assert
            Assert.Equal("/my/relative/path", result);
        }

        #endregion
    }
}
