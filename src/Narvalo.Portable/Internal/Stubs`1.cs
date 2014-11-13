// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static class Stubs<T>
    {
        static readonly Action<T> Ignore_ = _ => { };

        internal static Action<T> Ignore { get { return Ignore_; } }
    }
}
