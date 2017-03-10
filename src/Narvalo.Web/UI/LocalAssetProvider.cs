// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Web;

    using Narvalo.Collections;
    using Narvalo.Web.Properties;

    public sealed class LocalAssetProvider : AssetProvider
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
            DefaultDescription = Strings.LocalAssetProvider_Description;
        }

        public override Uri GetFontUri(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            return MakeUri(_fontsPath, relativePath);
        }

        public override Uri GetImageUri(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            return MakeUri(_imagesPath, relativePath);
        }

        public override Uri GetScriptUri(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            return MakeUri(_scriptsPath, relativePath);
        }

        public override Uri GetStyleUri(string relativePath)
        {
            // Here we can be sure that _fontsPath is not null or empty; cf. Initialize().
            return MakeUri(_stylesPath, relativePath);
        }

        // FIXME: If any value is invalid, return null.
        internal static string NormalizeAppRelativePath(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return value;
            }
        }

        protected override void InitializeCustom(NameValueCollection config)
        {
            //Maybe<string> fontsPath = config.MayGetSingle(FONTS_PATH_KEY);
            //Maybe<string> imagesPath = config.MayGetSingle(IMAGES_PATH_KEY);
            //Maybe<string> scriptsPath = config.MayGetSingle(SCRIPTS_PATH_KEY);
            //Maybe<string> stylesPath = config.MayGetSingle(STYLES_PATH_KEY);

            _fontsPath = config.MayGetSingle(FONTS_PATH_KEY)
                .Select(NormalizeAppRelativePath)
                .ValueOrElse("~/assets/fonts/");
            _imagesPath = config.MayGetSingle(IMAGES_PATH_KEY)
                .Select(NormalizeAppRelativePath)
                .ValueOrElse("~/assets/img/");
            _scriptsPath = config.MayGetSingle(SCRIPTS_PATH_KEY)
                .Select(NormalizeAppRelativePath)
                .ValueOrElse("~/assets/js/");
            _stylesPath = config.MayGetSingle(STYLES_PATH_KEY)
                .Select(NormalizeAppRelativePath)
                .ValueOrElse("~/assets/css/");

            //if (fontsPath.IsSome) { config.Remove(FONTS_PATH_KEY); }
            //if (imagesPath.IsSome) { config.Remove(IMAGES_PATH_KEY); }
            //if (scriptsPath.IsSome) { config.Remove(SCRIPTS_PATH_KEY); }
            //if (stylesPath.IsSome) { config.Remove(STYLES_PATH_KEY); }
        }

        private static Uri MakeUri(string baseIntermediatePath, string relativePath)
        {
            Require.NotNull(relativePath, nameof(relativePath));
            Debug.Assert(baseIntermediatePath != null && baseIntermediatePath.Length != 0);

            // NB: If basePath or relativePath is null or empty, VirtualPathUtility.Combine will throw,
            // which is of course exactly what we want.
            var uriString = VirtualPathUtility.ToAbsolute(
                relativePath.Length == 0
                ? baseIntermediatePath
                : VirtualPathUtility.Combine(baseIntermediatePath, relativePath));

            return new Uri(uriString, UriKind.Relative);
        }
    }
}
