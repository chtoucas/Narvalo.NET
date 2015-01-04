// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public class PrinterArgs : GhostScriptArgs<PrinterDevice>
    {
        public PrinterArgs(string inputFile)
            : base(inputFile, new PrinterDevice())
        {
        }
    }
}
