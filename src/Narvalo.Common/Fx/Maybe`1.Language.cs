namespace Narvalo.Fx
{
    using System;

    public partial struct Maybe<T>
    {
        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return Bind(_ => other);
        }

        public Maybe<T> Filter(Predicate<T> predicate)
        {
            return Map(_ => predicate(_)).Then(this);
        }

        public Maybe<Unit> When(bool predicate, MayFunc<Unit> kun)
        {
            return Bind(_ => kun.When(predicate).Invoke());
        }

        public Maybe<Unit> When(bool predicate, MayFunc<T, Unit> kun)
        {
            return Bind(kun.When(predicate));
        }

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
