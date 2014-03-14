// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.IO;

    public static class DirectoryInfoExtensions
    {
        static readonly string DirectorySeparator_ = Path.DirectorySeparatorChar.ToString();

        public static string GetRelativePathTo(this DirectoryInfo @this, string rootPath)
        {
            return GetRelativePathTo(@this, new Uri(rootPath));
        }

        public static string GetRelativePathTo(
            this DirectoryInfo @this, 
            DirectoryInfo rootDirectory)
        {
            return GetRelativePathTo(@this, new Uri(rootDirectory.GetNormalizedPath()));
        }

        internal static string GetRelativePathTo(this DirectoryInfo @this, Uri rootUri)
        {
            Require.Object(@this);
            Require.NotNull(rootUri, "rootUri");

            var pathUri = new Uri(@this.FullName);
            var relativeUri = rootUri.MakeRelativeUri(pathUri);

            return Uri.UnescapeDataString(
                relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        internal static string GetNormalizedPath(this DirectoryInfo @this)
        {
            Require.Object(@this);

            var path = @this.FullName;

            bool isNormalized = path.EndsWith(
                DirectorySeparator_,
                StringComparison.OrdinalIgnoreCase);

            return isNormalized ? path : path + DirectorySeparator_;
        }
    }
}
