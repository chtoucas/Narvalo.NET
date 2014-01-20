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
        public static Maybe<T> BindValue<T>(this Maybe<XAttribute> @this, Func<string, Maybe<T>> fun)
        {
            return @this.Bind(_ => fun.Invoke(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> @this, Func<string, T> fun)
        {
            return @this.Map(_ => fun.Invoke(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
