// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class NoopGuard : IGuard
    {
        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            action.Invoke();
        }
    }
}
