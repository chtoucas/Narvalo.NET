// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static class Stubs<T>
    {
        static readonly Action<T> Ignore_ = _ => { };
        static readonly Func<T, T> Identity_ = _ => _;
        static readonly Func<T, bool> True_ = _ => true;
        static readonly Func<T, bool> False_ = _ => false;

        public static Action<T> Ignore { get { return Ignore_; } }

        public static Func<T, T> Identity { get { return Identity_; } }

        public static Func<T, bool> False { get { return False_; } }

        public static Func<T, bool> True { get { return True_; } }
    }
}
