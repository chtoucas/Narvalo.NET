// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;

    using Xunit;

    public static class RemoteAssetProviderFacts
    {
        #region Initialize()

        [Fact]
        public static void Initialize_ThrowsProviderException_ForMissingBaseUri()
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(0);

            // Act & Assert
            Assert.Throws<ProviderException>(() => provider.Initialize("MyName", config));
        }

        [Fact]
        public static void Initialize_ThrowsProviderException_ForInvalidBaseUri()
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(1);
            config.Add("baseUri", "^//tempuri.org/my/relative/path");

            // Act & Assert
            Assert.Throws<ProviderException>(() => provider.Initialize("MyName", config));
        }

        [Fact]
        public static void Initialize_ThrowsProviderException_ForRelativeBaseUri()
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(1);
            config.Add("baseUri", "/my/relative/path");

            // Act & Assert
            Assert.Throws<ProviderException>(() => provider.Initialize("MyName", config));
        }

        [Fact]
        public static void Initialize_DoesNotThrow_ForAbsoluteBaseUri()
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(1);
            config.Add("baseUri", "http://tempuri.org/my/relative/path");

            // Act
            provider.Initialize("MyName", config);
        }

        #endregion

        #region GetFontUri()

        [Fact]
        public static void GetFontUri_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(1);
            config.Add("baseUri", "http://tempuri.org/");

            provider.Initialize("MyName", config);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetFontUri(null));
        }

        [Fact]
        public static void GetFontUri_ThrowsArgumentException_ForAbsolutePath()
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(1);
            config.Add("baseUri", "http://tempuri.org/");

            provider.Initialize("MyName", config);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => provider.GetFontUri("/my/absolute/font/path"));
        }

        [Theory]
        [InlineData("", "http://tempuri.org/fonts/")]
        [InlineData("path", "http://tempuri.org/fonts/path")]
        [InlineData("my/relative/path", "http://tempuri.org/fonts/my/relative/path")]
        [CLSCompliant(false)]
        public static void GetFontUri_ReturnsExpectedResult(string value, string expectedValue)
        {
            // Arrange
            var provider = new RemoteAssetProvider();
            var config = new NameValueCollection(1);
            config.Add("baseUri", "http://tempuri.org/");

            provider.Initialize("MyName", config);

            // Act
            var resultUri = provider.GetFontUri(value);

            // Act & Assert
            Assert.Equal(expectedValue, resultUri.ToString());
        }

        #endregion
    }
}
