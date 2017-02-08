// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Configuration.Provider;

    using Moq;
    using Xunit;

    public static class AssetProviderCollectionFacts
    {
        #region Add()

        [Fact]
        public static void Add_ThrowsArgumentNullException_ForNullInput()
        {
            // Arrange
            var providers = new AssetProviderCollection();
            ProviderBase provider = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => providers.Add(provider));
        }

        [Fact]
        public static void Add_ThrowsArgumentException_ForInvalidInput()
        {
            // Arrange
            var providers = new AssetProviderCollection();
            var provider = new Mock<ProviderBase>().Object;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => providers.Add(provider));
        }

        #endregion
    }
}
