namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Fournit des méthodes d'extension pour Maybe&lt;XAttribute&gt;.
    /// </summary>
    public static class MaybeXAttributeExtensions
    {
        public static Maybe<T> BindValue<T>(this Maybe<XAttribute> @this, Func<string, Maybe<T>> kun)
        {
            Require.NotNull(kun, "kun");

            return @this.Bind(_ => kun.Invoke(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, "selector");

            return @this.Map(_ => selector.Invoke(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
