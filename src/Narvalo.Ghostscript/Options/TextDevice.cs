// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public sealed class TextDevice : OutputDevice
    {
        public TextDevice() : base() { }

        public override bool CanDisplay => false;

        public override string DeviceName =>"txtwrite";
    }
}