// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static class Stubs
    {
        static readonly Action Noop_ = () => { };
        static readonly Action<Exception> Throw_ = _ => { throw _; };

        public static Action Noop { get { return Noop_; } }

        public static Action<Exception> Throw { get { return Throw_; } }
    }
}
