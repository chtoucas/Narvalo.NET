// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;

    using Narvalo.Collections;
    using Narvalo.Web.Properties;

    public sealed class RemoteAssetProvider : AssetProvider
    {
        // WARNING: Ne pas utiliser "/font/", car si _baseUri contient déjà un chemin relatif, il sera ignoré.
        internal const string DefaultFontsPath = "fonts/";
        internal const string DefaultImagesPath = "img/";
        internal const string DefaultScriptsPath = "js/";
        internal const string DefaultStylesPath = "css/";

        private const string BASE_URI_KEY = "baseUri";

        private Uri _baseUri;

        public RemoteAssetProvider()
        {
            DefaultName = "RemoteAssetProvider";
            DefaultDescription = Strings.RemoteAssetProvider_Description;
        }

        public override Uri GetFontUri(string relativePath)
        {
            return MakeUri(DefaultFontsPath, relativePath);
        }

        public override Uri GetImageUri(string relativePath)
        {
            return MakeUri(DefaultImagesPath, relativePath);
        }

        public override Uri GetScriptUri(string relativePath)
        {
            return MakeUri(DefaultScriptsPath, relativePath);
        }

        public override Uri GetStyleUri(string relativePath)
        {
            return MakeUri(DefaultStylesPath, relativePath);
        }

        protected override void InitializeCustom(NameValueCollection config)
        {
            _baseUri = config.MayGetSingle(BASE_URI_KEY)
                .Bind(_ => ParseTo.Uri(_, UriKind.Absolute))
                .ValueOrThrow(() => new ProviderException(Strings.RemoteAssetProvider_MissingOrInvalidBaseUri));

            config.Remove(BASE_URI_KEY);
        }

        private static string Combine(string basePath, string relativePath)
        {
            Demand.NotNull(basePath);
            Demand.Range(basePath.Length != 0);
            Demand.NotNull(relativePath);

            string retval;

            if (relativePath.Length == 0)
            {
                retval = basePath;
            }
            else if (relativePath[0] == '/')
            {
                // FIXME: Message = "relativePath" is not a relative path.
                throw new ArgumentOutOfRangeException(nameof(relativePath));
            }
            else if (HasTrailingSlash(basePath))
            {
                retval = basePath + relativePath;
            }
            else
            {
                retval = basePath + "/" + relativePath;
            }

            return retval;
        }

        private static bool HasTrailingSlash(string path)
        {
            Demand.Range(path.Length > 0);

            return path[path.Length - 1] == '/';
        }

        private Uri MakeUri(string baseIntermediatePath, string relativePath)
        {
            Require.NotNull(relativePath, nameof(relativePath));
            Demand.NotNull(baseIntermediatePath);
            Demand.Range(baseIntermediatePath.Length != 0);

            // Here we can be sure that _baseUri is not null and is absolute; otherwise an exception
            // would have been thrown in InitializeCustom().
            string relativeUri = Combine(baseIntermediatePath, relativePath);

            return new Uri(_baseUri, relativeUri);
        }
    }
}
