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
            return Match(fun, defaultValueFactory());
        }

        //// Then

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return Bind(_ => other);
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

        //// Run

        public void Run(Action<T> onSome, Action onNone)
        {
            Func<T, Unit> fun = _ => { onSome(_); return Unit.Single; };
            Func<Unit> factory = () => { onNone(); return Unit.Single; };

            Match(fun, factory);
        }

        //// OnSome

        public void OnSome(Action<T> action)
        {
            Run(action, () => { });
        }

        //// OnNone

        public void OnNone(Action action)
        {
            Run(_ => { }, action);
        }

        //// ThrowIfNone

        public Maybe<T> ThrowIfNone(Exception ex)
        {
            if (IsNone) {
                throw ex;
            }

            return this;
        }

        public Maybe<T> ThrowIfNone(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            return ThrowIfNone(exceptionFactory());
        }
    }
}
