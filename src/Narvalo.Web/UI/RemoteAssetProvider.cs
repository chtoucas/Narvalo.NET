// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Diagnostics.Contracts;
    using System.IO;

    using Narvalo.Collections;
    using Narvalo.Web.Properties;

    public sealed class RemoteAssetProvider : AssetProvider
    {
        // WARNING: Ne pas utiliser "/font/", car si _baseUri contient déjà un chemin relatif, il sera ignoré.
        internal static readonly string DefaultFontsPath = "fonts/";
        internal static readonly string DefaultImagesPath = "img/";
        internal static readonly string DefaultScriptsPath = "js/";
        internal static readonly string DefaultStylesPath = "css/";

        private const string BASE_URI_KEY = "baseUri";

        private Uri _baseUri;

        public RemoteAssetProvider()
        {
            DefaultName = "RemoteAssetProvider";
            DefaultDescription = Strings.RemoteAssetProvider_Description;
        }

        public override Uri GetFontUri(string relativePath)
        {
            return MakeUri_(DefaultFontsPath, relativePath);
        }

        public override Uri GetImageUri(string relativePath)
        {
            return MakeUri_(DefaultImagesPath, relativePath);
        }

        public override Uri GetScriptUri(string relativePath)
        {
            return MakeUri_(DefaultScriptsPath, relativePath);
        }

        public override Uri GetStyleUri(string relativePath)
        {
            return MakeUri_(DefaultStylesPath, relativePath);
        }

        protected override void InitializeCustom(NameValueCollection config)
        {
            InitializeCustomInternal(config);

            _baseUri = config.MayGetSingle(BASE_URI_KEY)
                .Bind(_ => ParseTo.Uri(_, UriKind.Absolute))
                .ValueOrThrow(() => new ProviderException(Strings.RemoteAssetProvider_MissingOrInvalidBaseUri));

            config.Remove(BASE_URI_KEY);
        }

        private static string Combine(string basePath, string relativePath)
        {
            Contract.Requires(basePath != null);
            Contract.Requires(basePath.Length != 0);
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<string>() != null);

            string result;

            if (relativePath.Length == 0)
            {
                result = basePath;
            }
            else if (relativePath[0] == '/')
            {
                // FIXME: Message = "relativePath" is not a relative path.
                throw new ArgumentOutOfRangeException();
            }
            else if (HasTrailingSlash(basePath))
            {
                result = basePath + relativePath;
            }
            else
            {
                result = basePath + "/" + relativePath;
            }

            return result;
        }

        private static bool HasTrailingSlash(string path)
        {
            Contract.Requires(path.Length > 0);

            return path[path.Length - 1] == '/';
        }

        private Uri MakeUri_(string baseIntermediatePath, string relativePath)
        {
            Require.NotNull(relativePath, "relativePath");
            Contract.Requires(baseIntermediatePath != null);
            Contract.Requires(baseIntermediatePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            // Here we can be sure that _baseUri is not null and is absolute; otherwise an exception 
            // would have been thrown in InitializeCustom().
            Contract.Assume(_baseUri != null);

            string relativeUri = Combine(baseIntermediatePath, relativePath);

            return new Uri(_baseUri, relativeUri);
        }
    }
}
