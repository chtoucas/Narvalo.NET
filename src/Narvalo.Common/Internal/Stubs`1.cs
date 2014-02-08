// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static class Stubs<T>
    {
        static readonly Action<T> Ignore_ = _ => { };
        static readonly Func<T, T> Identity_ = _ => _;

        public static Action<T> Ignore { get { return Ignore_; } }

        public static Func<T, T> Identity { get { return Identity_; } }
    }
}
