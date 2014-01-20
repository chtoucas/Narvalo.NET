namespace Narvalo.Fx
{
    using System;

    public partial struct Maybe<T>
    {
        //// Match

        public TResult Match<TResult>(Func<T, TResult> fun, TResult defaultValue)
        {
            return Map(fun).ValueOrElse(defaultValue);
        }

        public TResult Match<TResult>(Func<T, TResult> fun, Func<TResult> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return Match(fun, defaultValueFactory());
        }

        //// Then

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return Bind(_ => other);
        }

        //// OnSome

        public Maybe<T> OnSome(Action<T> action)
        {
            if (IsSome) {
                action(Value);
            }

            return this;
        }

        //// OnNone

        public Maybe<T> OnNone(Action action)
        {
            if (IsNone) {
                action();
            }

            return this;
        }

        //// Filter

        public Maybe<T> Filter(Predicate<T> predicate)
        {
            return Map(_ => predicate(_)).Then(this);
        }

        //// When

        public Maybe<Unit> When(bool predicate, MayFunc<Unit> kun)
        {
            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Maybe<Unit> When(bool predicate, MayFunc<T, Unit> kun)
        {
            return Bind(kun.When(predicate));
        }

        //// Unless

        public Maybe<Unit> Unless(bool predicate, MayFunc<Unit> kun)
        {
            return When(!predicate, kun);
        }

        public Maybe<Unit> Unless(bool predicate, MayFunc<T, Unit> kun)
        {
            return When(!predicate, kun);
        }
    }
}
