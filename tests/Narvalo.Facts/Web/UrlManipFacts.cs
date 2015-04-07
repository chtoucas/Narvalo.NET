// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;

    using Xunit;

    public static class UrlManipFacts
    {
        #region ToProtocolRelativeString()

        [Fact]
        public static void ToProtocolRelativeString_ThrowsArgumentNullException_ForNullInput()
        {
            // Arrange
            Uri uri = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => UrlManip.ToProtocolRelativeString(uri));
        }

        [Fact]
        public static void ToProtocolRelativeString_ThrowsNotSupportedException_ForUnsupportedScheme()
        {
            // Arrange
            Uri uri = new Uri("mailto:nobody@tempuri.org");

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => UrlManip.ToProtocolRelativeString(uri));
        }

        [Fact]
        public static void Link_ReturnsExpectedString_ForAbsoluteUriAndHttpScheme()
        {
            // Arrange
            Uri uri = new Uri("http://tempuri.org/my/relative/path");

            // Act
            var result = UrlManip.ToProtocolRelativeString(uri);

            // Assert
            Assert.Equal("//tempuri.org/my/relative/path", result);
        }

        [Fact]
        public static void Link_ReturnsExpectedString_ForAbsoluteUriAndHttpsScheme()
        {
            // Arrange
            Uri uri = new Uri("https://tempuri.org/my/relative/path");

            // Act
            var result = UrlManip.ToProtocolRelativeString(uri);

            // Assert
            Assert.Equal("//tempuri.org/my/relative/path", result);
        }

        [Fact]
        public static void Link_ReturnsExpectedString_ForRelativeUri()
        {
            // Arrange
            Uri uri = new Uri("/my/relative/path", UriKind.Relative);

            // Act
            var result = UrlManip.ToProtocolRelativeString(uri);

            // Assert
            Assert.Equal("/my/relative/path", result);
        }

        #endregion
    }
}
