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

