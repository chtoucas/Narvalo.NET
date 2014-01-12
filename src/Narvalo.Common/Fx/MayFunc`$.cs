namespace Narvalo.Fx
{
    using System;

    public static class MayFuncExtensions
    {
        public static Action AsAction(this MayFunc<Unit> @this)
        {
            Requires.Object(@this);

            return () => @this();
        }

        public static Action<T> AsAction<T>(this MayFunc<T, Unit> @this)
        {
            Requires.Object(@this);

            return _ => @this(_);
        }

        public static Func<T, Maybe<X>> AsFunc<T, X>(this MayFunc<T, X> @this)
        {
            Requires.Object(@this);

            return _ => @this(_);
        }

        #region + When & Unless +

        public static MayFunc<Unit> Unless(this MayFunc<Unit> @this, bool predicate)
        {
            Requires.Object(@this);

            return @this.When(!predicate);
        }

        public static MayFunc<T, Unit> Unless<T>(this MayFunc<T, Unit> @this, bool predicate)
        {
            Requires.Object(@this);

            return @this.When(!predicate);
        }

        public static MayFunc<Unit> When(this MayFunc<Unit> @this, bool predicate)
        {
            Requires.Object(@this);

            return predicate ? @this : () => Maybe.Unit;
        }

        public static MayFunc<T, Unit> When<T>(this MayFunc<T, Unit> @this, bool predicate)
        {
            Requires.Object(@this);

            return predicate ? @this : _ => Maybe.Unit;
        }

        #endregion
    }
}
