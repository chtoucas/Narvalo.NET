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
            Require.Object(@this);

            return () => @this();
        }

        public static Action<T> AsAction<T>(this MayFunc<T, Unit> @this)
        {
            Require.Object(@this);

            return _ => @this(_);
        }

        public static Func<T, Maybe<TResult>> AsFunc<T, TResult>(this MayFunc<T, TResult> @this)
        {
            Require.Object(@this);

            return _ => @this(_);
        }

        public static MayFunc<Unit> Unless(this MayFunc<Unit> @this, bool predicate)
        {
            Require.Object(@this);

            return @this.When(!predicate);
        }

        public static MayFunc<T, Unit> Unless<T>(this MayFunc<T, Unit> @this, bool predicate)
        {
            Require.Object(@this);

            return @this.When(!predicate);
        }

        public static MayFunc<Unit> When(this MayFunc<Unit> @this, bool predicate)
        {
            Require.Object(@this);

            return predicate ? @this : () => Maybe.Unit;
        }

        public static MayFunc<T, Unit> When<T>(this MayFunc<T, Unit> @this, bool predicate)
        {
            Require.Object(@this);

            return predicate ? @this : _ => Maybe.Unit;
        }
    }
}
