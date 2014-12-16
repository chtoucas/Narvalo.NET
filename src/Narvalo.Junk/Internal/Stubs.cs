// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static class Stubs
    {
        static readonly Action Noop_ = () => { };

        internal static Action Noop { get { return Noop_; } }
    }
}
