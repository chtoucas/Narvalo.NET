namespace Narvalo.Fx
{
    using System;

    public static class IOActionExtensions
    {
        public static Action AsAction(this Func<Unit> action)
        {
            return () => action();
        }

        public static Action<T> AsAction<T>(this Func<T, Unit> action)
        {
            return _ => action(_);
        }

        public static Func<Unit> AsFunc(this Func<Unit> action)
        {
            return () => action();
        }

        public static Func<T, Unit> AsFunc<T>(this Func<T, Unit> action)
        {
            return _ => action(_);
        }

        public static Func<Unit> When(this Func<Unit> action, bool predicate)
        {
            return predicate ? action : () => Unit.Single;
        }

        public static Func<T, Unit> When<T>(this Func<T, Unit> action, bool predicate)
        {
            return predicate ? action : _ => Unit.Single;
        }

        public static Func<Unit> Unless(this Func<Unit> action, bool predicate)
        {
            return action.When(!predicate);
        }

        public static Func<T, Unit> Unless<T>(this Func<T, Unit> action, bool predicate)
        {
            return action.When(!predicate);
        }
    }
}
