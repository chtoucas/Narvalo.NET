// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.IO;

    public static class PathUtility
    {
        static readonly string DirectorySeparator_ = Path.DirectorySeparatorChar.ToString();

        public static string MakeRelativePath(string rootPath, string path)
        {
            return MakeRelativePathInternal(new Uri(AppendDirectorySeparator(rootPath)), path);
        }

        // For this method to work correctly, the "rootUri" string must end with a "/".
        internal static string MakeRelativePathInternal(Uri rootUri, string path)
        {
            Require.NotNull(rootUri, "rootUri");

            var relativeUri = rootUri.MakeRelativeUri(new Uri(path));

            return Uri.UnescapeDataString(
                relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        internal static string AppendDirectorySeparator(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            bool endsWithSeparator 
                = path.EndsWith(DirectorySeparator_, StringComparison.OrdinalIgnoreCase);

            return endsWithSeparator ? path : path + DirectorySeparator_;
        }
    }
}
