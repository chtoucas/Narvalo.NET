namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Identity<T> : IEquatable<Identity<T>>, IEquatable<T>
    {
        public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> fun)
        {
            Requires.NotNull(fun, "fun");

            return fun(Value);
        }

        public Identity<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return Identity.Create(selector(Value));
        }
    }
}
