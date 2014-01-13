namespace Narvalo.Fx.Internal
{
    static class KuncStubs<T>
    {
        public static readonly Kunc<T, Unit> Ignore = _ => Monad.Unit;
    }
}
