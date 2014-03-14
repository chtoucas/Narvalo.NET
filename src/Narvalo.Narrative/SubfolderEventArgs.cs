// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;

    public sealed class SubfolderEventArgs : EventArgs
    {
        readonly string _relativePath;

        public SubfolderEventArgs(string relativePath)
        {
            _relativePath = relativePath;
        }

        public string RelativePath
        {
            get { return _relativePath; }
        }
    }

}
