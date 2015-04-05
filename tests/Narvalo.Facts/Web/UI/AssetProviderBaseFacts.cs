// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;

    using Narvalo.Web.Properties;
    using Xunit;

    public static partial class AssetProviderBaseFacts
    {
        private const string WHITESPACE_ONLY_STRING = "     ";

        private sealed class AssetProvider_ : AssetProviderBase
        {
            public override Uri GetFontUri(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetImageUri(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetScriptUri(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetStyleUri(string relativePath)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MyAssetProvider_ : AssetProviderBase
        {
            internal const string MyDefaultName = "MyDefaultName";
            internal const string MyDefaultDescription = "MyDefaultDescription";

            public MyAssetProvider_()
            {
                DefaultName = MyDefaultName;
                DefaultDescription = MyDefaultDescription;
            }

            public override Uri GetFontUri(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetImageUri(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetScriptUri(string relativePath)
            {
                throw new NotImplementedException();
            }

            public override Uri GetStyleUri(string relativePath)
            {
                throw new NotImplementedException();
            }
        }
    }

    public static partial class AssetProviderBaseFacts
    {
        #region Description

        [Fact]
        public static void Description_ReturnsCustomDefaultDescription()
        {
            // Arrange
            var provider = new MyAssetProvider_();

            // Act
            provider.Initialize(null, null);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultDescription, provider.Description);
        }

        [Fact]
        public static void Description_ReturnsDefaultDescription_ForNullDescription()
        {
            // Arrange
            var provider = new MyAssetProvider_();
            var config = new NameValueCollection(1);
            config.Add("description", null);

            // Act
            provider.Initialize(null, config);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultDescription, provider.Description);
        }

        [Fact]
        public static void Description_ReturnsDefaultDescription_ForEmptyDescription()
        {
            // Arrange
            var provider = new MyAssetProvider_();
            var config = new NameValueCollection(1);
            config.Add("description", String.Empty);

            // Act
            provider.Initialize(null, config);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultDescription, provider.Description);
        }

        [Fact]
        public static void Description_ReturnsDefaultDescription_ForWhiteSpaceOnlyDescription()
        {
            // Arrange
            var provider = new MyAssetProvider_();
            var config = new NameValueCollection(1);
            config.Add("description", WHITESPACE_ONLY_STRING);

            // Act
            provider.Initialize(null, config);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultDescription, provider.Description);
        }

        [Fact]
        public static void Description_ReturnsInputDescription()
        {
            // Arrange
            var provider = new AssetProvider_();
            var description = "MyDescription";
            var config = new NameValueCollection(1);
            config.Add("description", description);

            // Act
            provider.Initialize(null, config);

            // Assert
            Assert.Equal(description, provider.Description);
        }

        #endregion

        #region Name

        [Fact]
        public static void Name_ReturnsCustomDefaultName()
        {
            // Arrange
            var provider = new MyAssetProvider_();

            // Act
            provider.Initialize(null, null);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultName, provider.Name);
        }

        [Fact]
        public static void Name_ReturnsDefaultName_ForNullName()
        {
            // Arrange
            var provider = new MyAssetProvider_();

            // Act
            provider.Initialize(null, null);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultName, provider.Name);
        }

        [Fact]
        public static void Name_ReturnsDefaultName_ForEmptyName()
        {
            // Arrange
            var provider = new MyAssetProvider_();

            // Act
            provider.Initialize(String.Empty, null);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultName, provider.Name);
        }

        [Fact]
        public static void Name_ReturnsDefaultName_ForWhiteSpaceOnlyName()
        {
            // Arrange
            var provider = new MyAssetProvider_();

            // Act
            provider.Initialize(WHITESPACE_ONLY_STRING, null);

            // Assert
            Assert.Equal(MyAssetProvider_.MyDefaultName, provider.Name);
        }

        [Fact]
        public static void Name_ReturnsInputName_AfterInitialization()
        {
            // Arrange
            var provider = new AssetProvider_();
            var name = "MyName";

            // Act
            provider.Initialize(name, null);

            // Assert
            Assert.Equal(name, provider.Name);
        }

        #endregion

        #region Initialize()

        [Fact]
        public static void Initialize_ThrowsProviderException_ForUnknownConfigurationKey()
        {
            // Arrange
            var provider = new MyAssetProvider_();
            var config = new NameValueCollection(1);
            config.Add("description", "MyDescription");
            config.Add("myKey", "myValue");

            // Act & Assert
            Assert.Throws<ProviderException>(() => provider.Initialize("MyName", config));
        }

        #endregion
    }
    
#if NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class AssetProviderBaseFacts
    {
        [Fact(Skip = "White-box tests disabled for this configuration.")]
        public static void Description_BlackBox() { }
    
        [Fact(Skip = "White-box tests disabled for this configuration.")]
        public static void Name_BlackBox() { }
    }

#else

    public static partial class AssetProviderBaseFacts
    {
        #region Description

        [Fact]
        public static void Description_ReturnsInternalDefaultDescription()
        {
            // Arrange
            var provider = new AssetProvider_();

            // Act
            provider.Initialize(null, null);

            // Assert
            Assert.Equal(Strings_Web.AssetProviderBase_Description, provider.Description);
        }

        #endregion

        #region Name

        [Fact]
        public static void Name_ReturnsInternalDefaultName()
        {
            // Arrange
            var provider = new AssetProvider_();

            // Act
            provider.Initialize(null, null);

            // Assert
            Assert.Equal(AssetProviderBase.InternalDefaultName, provider.Name);
        }

        #endregion
    }

#endif
}
