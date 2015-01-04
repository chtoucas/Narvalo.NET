// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public class TextArgs : GhostScriptArgs<TextDevice>
    {
        public TextArgs(string inputFile)
            : base(inputFile, new TextDevice())
        {
        }
    }
}
