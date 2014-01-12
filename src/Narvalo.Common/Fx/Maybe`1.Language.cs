namespace Narvalo.Fx
{
    using System;

    public partial struct Maybe<T>
    {
        #region + Then +

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return Bind(_ => other);
        }

        #endregion

        #region + Filter +

        public Maybe<T> Filter(Predicate<T> predicate)
        {
            return Map(_ => predicate(_)).Then(this);
        }

        #endregion

        #region + When +

        public Maybe<Unit> When(bool predicate, MayFunc<Unit> kun)
        {
            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Maybe<Unit> When(bool predicate, MayFunc<T, Unit> kun)
        {
            return Bind(kun.When(predicate));
        }

        #endregion

        #region + Unless +

        public Maybe<Unit> Unless(bool predicate, MayFunc<Unit> kun)
        {
            return When(!predicate, kun);
        }

        public Maybe<Unit> Unless(bool predicate, MayFunc<T, Unit> kun)
        {
            return When(!predicate, kun);
        }

        #endregion
    }
}
