namespace Narvalo.Fx.Theory
{
    using System;

    public partial class Monad<T>
    {
        #region + SelectMany +

        public Monad<TResult> SelectMany<TMiddle, TResult>(
            Kunc<T, TMiddle> valueSelector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Requires.NotNull(valueSelector, "valueSelector");
            Requires.NotNull(resultSelector, "resultSelector");

            return Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }

        #endregion

        #region + Then +

        public Monad<TResult> Then<TResult>(Monad<TResult> other)
        {
            return Bind(_ => other);
        }

        #endregion

        #region + Filter +

        public Monad<T> Filter(Predicate<T> predicate)
        {
            Requires.NotNull(predicate, "predicate");

            return Map(_ => predicate(_)).Then(this);
        }

        #endregion

        #region + When +

        public Monad<Unit> When(bool predicate, Kunc<Unit> kun)
        {
            Requires.NotNull(kun, "@this");

            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Monad<Unit> When(bool predicate, Kunc<T, Unit> kun)
        {
            Requires.NotNull(kun, "@this");

            return Bind(kun.When(predicate));
        }

        #endregion

        #region + Unless +

        public Monad<Unit> Unless(bool predicate, Kunc<Unit> kun)
        {
            return When(!predicate, kun);
        }

        public Monad<Unit> Unless(bool predicate, Kunc<T, Unit> kun)
        {
            return When(!predicate, kun);
        }

        #endregion
    }
}
