// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public sealed class PassThroughSentinel : IReliabilitySentinel
    {
        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            action.Invoke();
        }
    }
}
