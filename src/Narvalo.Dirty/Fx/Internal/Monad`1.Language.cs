namespace Narvalo.Fx.Internal
{
    using System;

    partial class Monad<T>
    {
        public Monad<TResult> SelectMany<TMiddle, TResult>(
            Kunc<T, TMiddle> valueSelector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Requires.NotNull(valueSelector, "valueSelector");
            Requires.NotNull(resultSelector, "resultSelector");

            return Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }

        public Monad<TResult> Then<TResult>(Monad<TResult> other)
        {
            return Bind(_ => other);
        }

        public Monad<T> Filter(Predicate<T> predicate)
        {
            Requires.NotNull(predicate, "predicate");

            return Map(_ => predicate(_)).Then(this);
        }

        public Monad<Unit> When(bool predicate, Kunc<Unit> kun)
        {
            Requires.NotNull(kun, "kun");

            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Monad<Unit> When(bool predicate, Kunc<T, Unit> kun)
        {
            Requires.NotNull(kun, "kun");

            return Bind(kun.When(predicate));
        }

        public Monad<Unit> Unless(bool predicate, Kunc<Unit> kun)
        {
            return When(!predicate, kun);
        }

        public Monad<Unit> Unless(bool predicate, Kunc<T, Unit> kun)
        {
            return When(!predicate, kun);
        }
    }
}
