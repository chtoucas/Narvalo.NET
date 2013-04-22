namespace Narvalo.Fx.Theory
{
    using System;

    public static class MonadPlus
    {
        // Wrong...
        public static readonly Monad<Unit> Zero = Monad.Unit;

        public static Monad<T> Plus<T>(Monad<T> left, Monad<T> right)
        {
            throw new NotImplementedException();
        }

        public static Monad<Unit> Guard<T>(bool predicate)
        {
            return predicate ? Monad.Unit : Zero;
        }
    }
}
