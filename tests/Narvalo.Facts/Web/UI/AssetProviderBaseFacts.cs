// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;

    using Xunit;

    public static partial class AssetProviderBaseFacts
    {
        private sealed class AssetProvider_ : AssetProviderBase
        {
            public override Uri GetFont(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetImage(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetScript(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetStyle(string relativePath)
            {
                throw new NotImplementedException();
            }
        }
    }

    public static partial class AssetProviderBaseFacts
    {
        #region Initialize()

        [Fact]
        public static void Initialize_SetName()
        {
            // Arrange
            var provider = new AssetProvider_();
            var name = "MyProvider";

            // Act
            provider.Initialize(name, null);

            // Assert
            Assert.Equal(name, provider.Name);
        }

        #endregion
    }
}
