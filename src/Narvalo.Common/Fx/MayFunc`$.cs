namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="Narvalo.Fx.MayFunc{T}"/>.
    /// </summary>
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

        public static Func<T, Maybe<TResult>> AsFunc<T, TResult>(this MayFunc<T, TResult> @this)
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
