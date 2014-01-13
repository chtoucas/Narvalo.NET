namespace Narvalo.Fx.Internal
{
    using System;

    static class KuncExtensions
    {
        public static Action AsAction(this Kunc<Unit> kun)
        {
            return () => kun();
        }

        public static Action<T> AsAction<T>(this Kunc<T, Unit> kun)
        {
            return _ => kun(_);
        }

        public static Func<T, Monad<X>> AsFunc<T, X>(this Kunc<T, X> kun)
        {
            return _ => kun(_);
        }

        public static Kunc<Unit> Unless(this Kunc<Unit> kun, bool predicate)
        {
            return kun.When(!predicate);
        }

        public static Kunc<T, Unit> Unless<T>(this Kunc<T, Unit> kun, bool predicate)
        {
            return kun.When(!predicate);
        }

        public static Kunc<Unit> When(this Kunc<Unit> kun, bool predicate)
        {
            return predicate ? kun : () => Monad.Unit;
        }

        public static Kunc<T, Unit> When<T>(this Kunc<T, Unit> kun, bool predicate)
        {
            return predicate ? kun : _ => Monad.Unit;
        }
    }
}
