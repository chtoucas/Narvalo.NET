// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web.Properties;

    public sealed class LocalAssetProvider : AssetProviderBase
    {
        private const string FONTS_PATH_KEY = "fontsPath";
        private const string IMAGES_PATH_KEY = "imagesPath";
        private const string SCRIPTS_PATH_KEY = "scriptsPath";
        private const string STYLES_PATH_KEY = "stylesPath";

        private string _fontsPath;
        private string _imagesPath;
        private string _scriptsPath;
        private string _stylesPath;

        public LocalAssetProvider()
        {
            DefaultName = "LocalAssetProvider";
            DefaultDescription = Strings_Web.LocalAssetProvider_Description;
        }

        public override Uri GetFont(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            Contract.Assume(_fontsPath != null);
            Contract.Assume(_fontsPath.Length != 0);

            return MakeUri_(_fontsPath, relativePath);
        }

        public override Uri GetImage(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            Contract.Assume(_imagesPath != null);
            Contract.Assume(_imagesPath.Length != 0);

            return MakeUri_(_imagesPath, relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            Contract.Assume(_scriptsPath != null);
            Contract.Assume(_scriptsPath.Length != 0);

            return MakeUri_(_scriptsPath, relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            Contract.Assume(_stylesPath != null);
            Contract.Assume(_stylesPath.Length != 0);

            return MakeUri_(_stylesPath, relativePath);
        }

        protected override void InitializeCustom(NameValueCollection config)
        {
            // This is guaranteed by base.Initialize().
            Contract.Assume(config != null);

            var fontsPath = config.MayGetSingle(FONTS_PATH_KEY);
            var imagesPath = config.MayGetSingle(IMAGES_PATH_KEY);
            var scriptsPath = config.MayGetSingle(SCRIPTS_PATH_KEY);
            var stylesPath = config.MayGetSingle(STYLES_PATH_KEY);

            _fontsPath = fontsPath.Where(NotWhiteSpace_).ValueOrElse("~/assets/fonts/");
            _imagesPath = imagesPath.Where(NotWhiteSpace_).ValueOrElse("~/assets/img/");
            _scriptsPath = scriptsPath.Where(NotWhiteSpace_).ValueOrElse("~/assets/js/");
            _stylesPath = stylesPath.Where(NotWhiteSpace_).ValueOrElse("~/assets/css/");

            if (fontsPath.IsSome) { config.Remove(FONTS_PATH_KEY); }
            if (imagesPath.IsSome) { config.Remove(IMAGES_PATH_KEY); }
            if (scriptsPath.IsSome) { config.Remove(SCRIPTS_PATH_KEY); }
            if (stylesPath.IsSome) { config.Remove(STYLES_PATH_KEY); }
        }

        private static bool NotWhiteSpace_(string value)
        {
            return !String.IsNullOrWhiteSpace(value);
        }

        private static Uri MakeUri_(string basePath, string relativePath)
        {
            Contract.Requires(basePath != null);
            Contract.Requires(basePath.Length != 0);
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            // NB: If basePath or relativePath is null or empty, VirtualPathUtility.Combine will throw,
            // which is of course exactly what we want.
            var uriString = VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(basePath, relativePath));

            return new Uri(uriString, UriKind.Relative);
        }
    }
}
