// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript
{
    using Narvalo.GhostScript.Options;

    public interface IGhostScriptApi
    {
        void Execute<T>(GhostScriptArgs<T> args) where T : Device;
        void Execute(string[] args);
    }
}
