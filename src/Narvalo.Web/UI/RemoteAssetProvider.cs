// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Diagnostics.Contracts;

    using Narvalo.Collections;
    using Narvalo.Web.Properties;

    public sealed class RemoteAssetProvider : AssetProviderBase
    {
        private const string BASE_URI_KEY = "baseUri";

        private Uri _baseUri;

        public RemoteAssetProvider()
        {
            DefaultName = "RemoteAssetProvider";
            DefaultDescription = Strings_Web.RemoteAssetProvider_Description;
        }

        public override Uri GetFont(string relativePath)
        {
            // WARNING: Ne pas utiliser "/font/", car si _baseUri contient déjà un chemin relatif, il sera ignoré.
            return MakeUri_("fonts/", relativePath);
        }

        public override Uri GetImage(string relativePath)
        {
            // WARNING: Ne pas utiliser "/img/", car si _baseUri contient déjà un chemin relatif, il sera ignoré.
            return MakeUri_("img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            // WARNING: Ne pas utiliser "/js/", car si _baseUri contient déjà un chemin relatif, il sera ignoré.
            return MakeUri_("js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            // WARNING: Ne pas utiliser "/css/", car si _baseUri contient déjà un chemin relatif, il sera ignoré.
            return MakeUri_("css/", relativePath);
        }

        protected override void InitializeCustom(NameValueCollection config)
        {
            InitializeCustomInternal(config);

            _baseUri = config.MayGetSingle(BASE_URI_KEY)
                .Bind(_ => ParseTo.Uri(_, UriKind.RelativeOrAbsolute))
                .ValueOrThrow(() => new ProviderException(Strings_Web.RemoteAssetProvider_MissingOrInvalidBaseUri));

            config.Remove(BASE_URI_KEY);
        }

        private Uri MakeUri_(string basePath, string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Requires(basePath != null);
            Contract.Requires(basePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            // Here we can be sure that _baseUri is not null; otherwise an exception 
            // would have been thrown in Initialize().
            Contract.Assume(_baseUri != null);

            // FIXME: Use Path.Combine?
            return new Uri(_baseUri, basePath + relativePath);
        }
    }
}
