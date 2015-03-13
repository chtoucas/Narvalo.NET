// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    public interface IBarrier : ISentinel
    {
        bool CanExecute { get; }
    }
}
