namespace Narvalo.Fx.Internal
{
    using System;

    partial class Monad<T>
    {
        //// SelectMany

        public Monad<TResult> SelectMany<TMiddle, TResult>(
            Kunc<T, TMiddle> valueSelector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }

        //// Then

        public Monad<TResult> Then<TResult>(Monad<TResult> other)
        {
            return Bind(_ => other);
        }

        //// Filter

        public Monad<T> Filter(Predicate<T> predicate)
        {
            Require.NotNull(predicate, "predicate");

            return Map(_ => predicate(_)).Then(this);
        }

        //// When

        public Monad<Unit> When(bool predicate, Kunc<Unit> kun)
        {
            Require.NotNull(kun, "kun");

            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Monad<Unit> When(bool predicate, Kunc<T, Unit> kun)
        {
            Require.NotNull(kun, "kun");

            return Bind(kun.When(predicate));
        }

        //// Unless

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
