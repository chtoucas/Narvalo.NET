// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;

    public sealed class SubfolderEventArgs : EventArgs
    {
        readonly string _relativePath;

        public SubfolderEventArgs(string relativePath)
        {
            Require.NotNull(relativePath, "relativePath");

            _relativePath = relativePath;
        }

        public string RelativePath
        {
            get { return _relativePath; }
        }
    }
}
