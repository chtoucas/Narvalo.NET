namespace Narvalo.Fx
{
    using System;

    public partial class Maybe<T>
    {
        #region Standard

        //// Filter

        public Maybe<T> Filter(Func<T, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            return Bind(_ => predicate.Invoke(_) ? this : Maybe<T>.None);
        }

        //// Then

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return Bind(_ => other);
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
