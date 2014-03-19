// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using Narvalo.IO;

    public interface IPathProvider
    {
        string GetDirectoryPath(RelativeDirectory directory);

        string GetFilePath(RelativeFile file);
    }
}
