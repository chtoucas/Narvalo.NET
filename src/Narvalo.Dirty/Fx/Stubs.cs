namespace Narvalo.Fx
{
    using System;

    public static class Stubs
    {
        public static readonly Action Noop = () => { };

        public static readonly Action<Exception> Throw = _ => { throw _; };
    }
}
