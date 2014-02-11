// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    static class Stubs
    {
        static readonly Kunc<Unit, Unit> Noop_ = _ => Monad.Unit;

        public static Kunc<Unit, Unit> Noop { get { return Noop_; } }
    }

    static class Stubs<T>
    {
        static readonly Kunc<T, Unit> Ignore_ = _ => Monad.Unit;
        static readonly Kunc<T, bool> True_ = _ => Monad.Return(true);
        static readonly Kunc<T, bool> False_ = _ => Monad.Return(false);

        public static Kunc<T, Unit> Ignore { get { return Ignore_; } }

        public static Kunc<T, bool> False { get { return False_; } }

        public static Kunc<T, bool> True { get { return True_; } }
    }
}
