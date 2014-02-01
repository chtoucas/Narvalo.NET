namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Xml.Linq.XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        public static T MapValue<T>(this XAttribute @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector(@this.Value);
        }

        public static Maybe<T> MayParseValue<T>(this XAttribute @this, Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);
            Require.NotNull(parserM, "fun");

            return parserM.Invoke(@this.Value);
        }
    }
}
