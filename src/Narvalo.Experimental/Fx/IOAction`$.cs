namespace Narvalo.Fx
{
    using System;

    public static class IOActionExtensions
    {
        public static Action AsAction(this IOAction action)
        {
            return () => action();
        }

        public static Action<T> AsAction<T>(this IOAction<T> action)
        {
            return _ => action(_);
        }

        public static Func<Unit> AsFunc(this IOAction action)
        {
            return () => action();
        }

        public static Func<T, Unit> AsFunc<T>(this IOAction<T> action)
        {
            return _ => action(_);
        }

        #region + When & Unless +

        public static IOAction When(this IOAction action, bool predicate)
        {
            return predicate ? action : () => Unit.Single;
        }

        public static IOAction<T> When<T>(this IOAction<T> action, bool predicate)
        {
            return predicate ? action : _ => Unit.Single;
        }

        public static IOAction Unless(this IOAction action, bool predicate)
        {
            return action.When(!predicate);
        }

        public static IOAction<T> Unless<T>(this IOAction<T> action, bool predicate)
        {
            return action.When(!predicate);
        }

        #endregion
    }
}
