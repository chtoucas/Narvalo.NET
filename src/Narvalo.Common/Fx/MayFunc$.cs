namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Fournit des méthodes d'extension pour Narvalo.Fx.Func&lt;Maybe&lt;T&gt;&gt;.
    /// </summary>
    public static class MayFuncExtensions
    {
        public static Action AsAction(this Func<Maybe<Unit>> @this)
        {
            Require.Object(@this);

            return () => @this.Invoke();
        }

        public static Action<T> AsAction<T>(this Func<T, Maybe<Unit>> @this)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_);
        }

        public static Func<T, Maybe<TResult>> AsFunc<T, TResult>(this Func<T, Maybe<TResult>> @this)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_);
        }

        public static Func<Maybe<Unit>> Unless(this Func<Maybe<Unit>> @this, bool predicate)
        {
            Require.Object(@this);

            return @this.When(!predicate);
        }

        public static Func<T, Maybe<Unit>> Unless<T>(this Func<T, Maybe<Unit>> @this, bool predicate)
        {
            Require.Object(@this);

            return @this.When(!predicate);
        }

        public static Func<Maybe<Unit>> When(this Func<Maybe<Unit>> @this, bool predicate)
        {
            Require.Object(@this);

            return predicate ? @this : () => Maybe.Unit;
        }

        public static Func<T, Maybe<Unit>> When<T>(this Func<T, Maybe<Unit>> @this, bool predicate)
        {
            Require.Object(@this);

            return predicate ? @this : _ => Maybe.Unit;
        }
    }
}
