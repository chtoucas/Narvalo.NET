namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for Maybe&lt;XElement&gt;.
    /// </summary>
    public static class MaybeXElementExtensions
    {
        public static Maybe<T> BindValue<T>(this Maybe<XElement> @this, Func<string, Maybe<T>> kun)
        {
            Require.NotNull(kun, "kun");

            return @this.Bind(_ => kun.Invoke(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, "selector");

            return @this.Map(_ => selector.Invoke(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
