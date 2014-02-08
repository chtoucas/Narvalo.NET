namespace Narvalo.Fx.Internal
{
    using System;

    static class KuncAction
    {
        static readonly Kunc<Unit> Noop_ = () => Monad.Unit;

        public static Kunc<Unit> Noop { get { return Noop_; } }

        public static Kunc<T, Unit> Ignore<T>()
        {
            return _ => Monad.Unit;
        }

        public static Kunc<Unit> FromAction(Action action)
        {
            return () => { action(); return Monad.Unit; };
        }

        public static Kunc<T, Unit> FromAction<T>(Action<T> action)
        {
            return _ => { action(_); return Monad.Unit; };
        }
    }
}
