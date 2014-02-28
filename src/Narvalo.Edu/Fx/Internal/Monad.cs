namespace Narvalo.Edu.Fx.Internal
{
    using Narvalo.Fx;

    static partial class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Unit { get { return Unit_; } }

        // [Haskell] return
        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }
    }
}
