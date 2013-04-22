namespace Narvalo.Fx.Theory
{
    using System;

    public static class Kunc
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
