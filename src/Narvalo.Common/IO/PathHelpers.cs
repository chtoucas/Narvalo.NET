// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    public static class PathHelpers
    {
        private static readonly string s_DirectorySeparator = Path.DirectorySeparatorChar.ToString();

        public static string MakeRelativePath(string rootPath, string path)
        {
            Expect.NotNull(rootPath);
            Expect.NotNull(path);
            Warrant.NotNull<string>();

            return MakeRelativePathCore(new Uri(AppendDirectorySeparator(rootPath)), path);
        }

        // For this method to work correctly, the "rootUri" string must end with a "/".
        internal static string MakeRelativePathCore(Uri rootUri, string path)
        {
            Require.NotNull(rootUri, nameof(rootUri));
            Expect.NotNull(path);
            Warrant.NotNull<string>();

            var relativeUri = rootUri.MakeRelativeUri(new Uri(path));

            return Uri.UnescapeDataString(
                relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        internal static string AppendDirectorySeparator(string path)
        {
            // REVIEW: Require.NotNullOrEmpty(path, "path");
            Require.NotNull(path, nameof(path));

            bool endsWithSeparator
                = path.EndsWith(s_DirectorySeparator, StringComparison.OrdinalIgnoreCase);

            return endsWithSeparator ? path : path + s_DirectorySeparator;
        }
    }
}
