// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;

    [Flags]
    public enum Interactions
    {
        None = 0x0,

        Batch,
        NoPagePrompt,
        NoPause,
        NoPrompt,

        /// <summary>
        /// Suppresses routine information comments on standard output. This is currently 
        /// necessary when redirecting device output to standard output.
        /// </summary>
        Quiet,

        /// <summary>
        /// Quiet startup: suppress normal startup messages, and also do the equivalent of -dQUIET.
        /// </summary>
        QuietStartup,

        ShortErrors,

        // StandardOutput,
        TtyPause,

        Default = Batch | NoPause,
    }
}
