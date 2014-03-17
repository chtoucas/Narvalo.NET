// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;

    public interface IWeaver
    {
        void Weave(FileInfo file, string outputPath);
    }
}
