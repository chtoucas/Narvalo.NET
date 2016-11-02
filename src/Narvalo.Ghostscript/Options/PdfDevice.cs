// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;

    public sealed class PdfDevice : OutputDevice
    {
        public PdfDevice() : base() { }

        public override bool CanDisplay => true;

        public override string DeviceName => "pdfwrite";

        public override void AddDisplayTo(ICollection<string> args)
        {
            throw new NotImplementedException();
        }
    }
}
