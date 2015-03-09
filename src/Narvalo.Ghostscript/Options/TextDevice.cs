// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public class TextDevice : OutputDevice
    {
        public TextDevice() : base() { }

        public override bool CanDisplay { get { return false; } }

        public override string DeviceName
        {
            get { return "txtwrite"; }
        }
    }
}