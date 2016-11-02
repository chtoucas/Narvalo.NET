// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript
{
    using System;

    using Narvalo.GhostScript.Internal;

    public static class GhostScriptApiFactory
    {
        public static IGhostScriptApi CreateApi()
        {
            // FIXME: utiliser Environment.Is64BitProcess
            if (IntPtr.Size == 8)
            {
                return new Win64GhostScriptApi();
            }
            else if (IntPtr.Size == 4)
            {
                return new Win32GhostScriptApi();
            }
            else
            {
                throw new PlatformNotSupportedException("XXX");
            }
        }
    }
}
