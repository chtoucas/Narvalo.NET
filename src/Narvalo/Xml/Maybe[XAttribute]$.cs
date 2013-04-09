namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Linq;
    using Narvalo.Fx;

    public static class MaybeXAttributeExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<T> BindValue<T>(this Maybe<XAttribute> attr, Func<string, Maybe<T>> fun)
        {
            return attr.Bind(_ => fun(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> attr, Func<string, T> fun)
        {
            return attr.Map(_ => fun(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> attr)
        {
            return attr.MapValue(_ => _);
        }
    }
}
