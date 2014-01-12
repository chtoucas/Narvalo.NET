namespace Narvalo.Internal
{
    using System;

    static class Stubs
    {
        public static readonly Action Noop = () => { };

        public static readonly Action<Exception> Throw = _ => { throw _; };
    }
}
