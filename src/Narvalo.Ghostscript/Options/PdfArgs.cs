// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public class PdfArgs : GhostScriptArgs<PdfDevice>
    {
        public PdfArgs(string inputFile)
            : base(inputFile, new PdfDevice())
        {
        }
    }
}
