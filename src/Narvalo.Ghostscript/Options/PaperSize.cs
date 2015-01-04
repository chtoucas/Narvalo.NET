// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public enum PaperSize
    {
        None = 0,             // Not a real value, just an instruction to whatever is making use of this enum

        // US
        ArchE,
        ArchD,
        ArchC,
        ArchB,
        ArchA,
        Ledger,
        LedgerPortrait,
        Legal,
        Letter,
        LetterSmall,

        // ISO
        A0,
        A1,
        A2,
        A3,
        A4,
        A5,
        A6,
        A7,
        A8,
        A9,
        A10,
        B0,
        B1,
        B2,
        B3,
        B4,
        B5,
        B6,
        C0,
        C1,
        C2,
        C3,
        C4,
        C5,
        C6,

        // Other
        Foolscap,
        Hagaki,
        HalfLetter,
    }
}
