// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.IO;

    public static class PathUtility
    {
        static readonly string DirectorySeparator_ = Path.DirectorySeparatorChar.ToString();

        public static string GetNormalizedPath(DirectoryInfo directory)
        {
            return AppendDirectorySeparator_(directory.FullName);
        }

        public static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public static string MakeRelativePath(string rootPath, string path)
        {
            return MakeRelativePathInternal(new Uri(AppendDirectorySeparator_(rootPath)), path);
        }

        internal static string MakeRelativePathInternal(Uri rootUri, string path)
        {
            Require.NotNull(rootUri, "rootUri");

            var relativeUri = rootUri.MakeRelativeUri(new Uri(path));

            return Uri.UnescapeDataString(
                relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        static string AppendDirectorySeparator_(string path)
        {
            bool isNormalized = path.EndsWith(
                DirectorySeparator_,
                StringComparison.OrdinalIgnoreCase);

            return isNormalized ? path : path + DirectorySeparator_;
        }
    }
}
