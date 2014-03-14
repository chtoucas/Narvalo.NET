// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.IO;

    internal static class DirectoryInfoExtensions
    {
        public static string GetNormalizedPath(this DirectoryInfo @this)
        {
            Require.Object(@this);

            var path = @this.FullName;

            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        public static string GetRelativePathTo(this DirectoryInfo @this, Uri rootUri)
        {
            Require.Object(@this);
            Require.NotNull(rootUri, "rootUri");

            var pathUri = new Uri(@this.FullName);
            var relativeUri = rootUri.MakeRelativeUri(pathUri);

            return Uri.UnescapeDataString(
                relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
