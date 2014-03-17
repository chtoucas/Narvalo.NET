// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    public interface IRunner
    {
        bool DryRun { get; set; }

        void Run();
    }
}
