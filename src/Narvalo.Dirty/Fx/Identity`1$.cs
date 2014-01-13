namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class IdentityExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Identity<TResult> Bind<TSource, TResult>(
            this Identity<TSource> id,
            Func<TSource, Identity<TResult>> fun)
        {
            Requires.NotNull(fun, "fun");

            return fun(id.Value);
        }

        public static Identity<TResult> Map<TSource, TResult>(
            this Identity<TSource> id,
            Func<TSource, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return Identity.Create(selector(id.Value));
        }

        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //public static Identity<TResult> SelectMany<TSource, TInner, TResult>(
        //    this Identity<TSource> id,
        //    Func<TSource, Identity<TInner>> valueSelector,
        //    Func<TSource, TInner, TResult> resultSelector)
        //{
        //    Requires.NotNull(valueSelector, "valueSelector");
        //    Requires.NotNull(resultSelector, "resultSelector");

        //    return id.Bind(t => valueSelector(t).Select(m => resultSelector(t, m)));
        //}
    }
}
