// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;

    [Flags]
    public enum Miscs
    {
        None = 0x0,

        DelayBind,
        PdfMarks,
        JobServer,
        NoBind,
        NoCache,
        NoGC,
        NoOuterSave,
        // FIXME
        NoSafer,
        DelayedSave = NoSafer,
        Safer,
        Strict,
        SystemDictionaryWritable,

        Default = Safer,
    }
}

