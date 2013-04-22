namespace Narvalo.Fx.Theory
{
    public static class KuncStubs<T>
    {
        public static readonly Kunc<T, Unit> Ignore = _ => Monad.Unit;
    }

}
