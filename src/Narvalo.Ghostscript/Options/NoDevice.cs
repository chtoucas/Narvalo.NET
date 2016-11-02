// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System.Collections.Generic;

    public sealed class NoDevice : Device
    {
        public NoDevice() : base() { }

        public override void AddTo(ICollection<string> args)
        {
            Require.NotNull(args, nameof(args));

            args.Add("-dNODISPLAY");
        }
    }
}
