// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;

    public sealed class PrinterDevice : OutputDevice
    {
        public PrinterDevice() : base() { }

        public override bool CanDisplay => true;

        public override string DeviceName
        {
            get { throw new NotImplementedException(); }
        }

        public override void AddDisplayTo(ICollection<string> args)
        {
            throw new NotImplementedException();
        }
    }
}
