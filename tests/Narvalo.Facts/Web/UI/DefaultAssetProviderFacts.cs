// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;

    using Xunit;

    // TODO: Test absolute, relative and bad paths.
    public static class DefaultAssetProviderFacts
    {
        #region GetFontUri()

        [Fact]
        public static void GetFontUri_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            var provider = new DefaultAssetProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetFontUri(null));
        }

        [Fact]
        [Trait("Unsafe", "AppDomain")]
        public static void GetFontUri_ReturnsBasePath_ForEmptyPath()
        {
            AppDomainUtility.RunInSeparateAppDomain(() =>
            {
                AspNetUtility.SetupAspNetDomain();

                using (var _ = AspNetUtility.CreateHttpContext())
                {
                    // Arrange
                    var provider = new DefaultAssetProvider();

                    // Act
                    var uri = provider.GetFontUri(String.Empty);

                    // Act & Assert
                    Assert.Equal("/fonts/", uri.ToString());
                }
            });
        }

        #endregion

        #region GetImageUri()

        [Fact]
        public static void GetImageUri_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            var provider = new DefaultAssetProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetImageUri(null));
        }

        [Fact]
        [Trait("Unsafe", "AppDomain")]
        public static void GetImageUri_ReturnsBasePath_ForEmptyPath()
        {
            AppDomainUtility.RunInSeparateAppDomain(() =>
            {
                AspNetUtility.SetupAspNetDomain();

                using (var _ = AspNetUtility.CreateHttpContext())
                {
                    // Arrange
                    var provider = new DefaultAssetProvider();

                    // Act
                    var uri = provider.GetImageUri(String.Empty);

                    // Act & Assert
                    Assert.Equal("/Images/", uri.ToString());
                }
            });
        }

        #endregion

        #region GetScriptUri()

        [Fact]
        public static void GetScriptUri_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            var provider = new DefaultAssetProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetScriptUri(null));
        }

        [Fact]
        [Trait("Unsafe", "AppDomain")]
        public static void GetScriptUri_ReturnsBasePath_ForEmptyPath()
        {
            AppDomainUtility.RunInSeparateAppDomain(() =>
            {
                AspNetUtility.SetupAspNetDomain();

                using (var _ = AspNetUtility.CreateHttpContext())
                {
                    // Arrange
                    var provider = new DefaultAssetProvider();

                    // Act
                    var uri = provider.GetScriptUri(String.Empty);

                    // Act & Assert
                    Assert.Equal("/Scripts/", uri.ToString());
                }
            });
        }

        #endregion

        #region GetStyleUri()

        [Fact]
        public static void GetStyleUri_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            var provider = new DefaultAssetProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetStyleUri(null));
        }

        [Fact]
        [Trait("Unsafe", "AppDomain")]
        public static void GetStyleUri_ReturnsBasePath_ForEmptyPath()
        {
            AppDomainUtility.RunInSeparateAppDomain(() =>
            {
                AspNetUtility.SetupAspNetDomain();

                using (var _ = AspNetUtility.CreateHttpContext())
                {
                    // Arrange
                    var provider = new DefaultAssetProvider();

                    // Act
                    var uri = provider.GetStyleUri(String.Empty);

                    // Act & Assert
                    Assert.Equal("/Content/", uri.ToString());
                }
            });
        }

        #endregion
    }
}
