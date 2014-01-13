namespace Narvalo.Fx.Theory
{
    using System;

    public class MonadPlus<T> : Monad<T>
    {
        protected MonadPlus(T value) : base(value) { }

        public static MonadPlus<T> Plus(MonadPlus<T> left, MonadPlus<T> right)
        {
            //    if (optionA.IsSome) {
            //        return optionA;
            //    }
            //    else {
            //        return optionB;
            //    }
            throw new NotImplementedException();
        }
    }
}