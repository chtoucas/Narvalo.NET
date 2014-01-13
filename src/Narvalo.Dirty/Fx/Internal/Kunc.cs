namespace Narvalo.Fx.Internal
{
    using System;

    static class Kunc
    {
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
