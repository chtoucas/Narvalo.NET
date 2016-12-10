// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;

    public sealed class UnitTestTraceListener : DefaultTraceListener
    {
        public override void Fail(string message)
        {
            throw new DebugAssertFailedException();
        }

        public override void Fail(string message, string detailMessage)
        {
            throw new DebugAssertFailedException();
        }
    }
}
