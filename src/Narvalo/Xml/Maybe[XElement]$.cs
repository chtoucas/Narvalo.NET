namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Linq;
    using Narvalo.Fx;

    public static class MaybeXElementExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<T> BindValue<T>(this Maybe<XElement> element, Func<string, Maybe<T>> fun)
        {
            return element.Bind(_ => fun(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XElement> element, Func<string, T> fun)
        {
            return element.Map(_ => fun(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> element)
        {
            return element.MapValue(_ => _);
        }
    }
}
