namespace Narvalo.Fx
{
    using System;

    public partial class Maybe<T>
    {
        #region Standard

        //// Match

        public TResult Match<TResult>(Func<T, TResult> selector, TResult defaultValue)
        {
            return Map(selector).ValueOrElse(defaultValue);
        }

        public TResult Match<TResult>(Func<T, TResult> selector, Func<TResult> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return Match(selector, defaultValueFactory.Invoke());
        }

        //// SelectMany

        public Maybe<TResult> SelectMany<TMiddle, TResult>(
            Func<T, Maybe<TMiddle>> valueSelector,
            Func<T, TMiddle, Maybe<TResult>> resultSelector)
        {
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            Maybe<Maybe<TResult>> result = Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));

            return Maybe.Join(result);
        }

        //// Then

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return Bind(_ => other);
        }

        //// Filter

        public Maybe<T> Filter(Predicate<T> predicate)
        {
            return Map(_ => predicate.Invoke(_)).Then(this);
        }

        //// When

        public Maybe<Unit> When(bool predicate, Func<Maybe<Unit>> kun)
        {
            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Maybe<Unit> When(bool predicate, Func<T, Maybe<Unit>> kun)
        {
            return Bind(kun.When(predicate));
        }

        //// Unless

        public Maybe<Unit> Unless(bool predicate, Func<Maybe<Unit>> kun)
        {
            return When(!predicate, kun);
        }

        public Maybe<Unit> Unless(bool predicate, Func<T, Maybe<Unit>> kun)
        {
            return When(!predicate, kun);
        }

        #endregion

        //// OnSome

        public Maybe<T> OnSome(Action<T> action)
        {
            Require.NotNull(action, "action");

            if (IsSome) {
                action.Invoke(Value);
            }

            return this;
        }

        //// OnNone

        public Maybe<T> OnNone(Action action)
        {
            Require.NotNull(action, "action");

            if (IsNone) {
                action.Invoke();
            }

            return this;
        }

        //// TrySet

        public bool TrySet(out T value)
        {
            if (IsSome) {
                value = Value;
                return true;
            }
            else {
                value = default(T);
                return false;
            }
        }
    }
}
