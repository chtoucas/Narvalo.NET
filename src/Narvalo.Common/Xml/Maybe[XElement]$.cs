namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    public static class MaybeXElementExtensions
    {
        public static Maybe<T> BindValue<T>(this Maybe<XElement> @this, Func<string, Maybe<T>> fun)
        {
            return @this.Bind(_ => fun(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> fun)
        {
            return @this.Map(_ => fun(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
