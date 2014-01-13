namespace Narvalo.Internal
{
    using System;

    static class Stubs<T>
    {
        public static readonly Action<T> Ignore = _ => { };

        public static readonly Func<T, T> Identity = _ => _;
    }
}
