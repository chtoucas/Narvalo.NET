// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    public class BlackholeGuard : ISentinel
    {
        public void Execute(Action action)
        {
        }
    }
}
