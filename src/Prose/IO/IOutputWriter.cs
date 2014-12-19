// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.IO
{
    using System.IO;
    using Narvalo.IO;

    public interface IOutputWriter
    {
        void CreateDirectory(RelativeDirectory directory);

        void Write(RelativeFile file, string content);

        void Write(FileInfo file, string content);
    }
}
