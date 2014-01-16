namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Linq;
    using Narvalo.Fx;

    public static class MaybeXAttributeExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<T> BindValue<T>(this Maybe<XAttribute> @this, Func<string, Maybe<T>> fun)
        {
            return @this.Bind(_ => fun(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> @this, Func<string, T> fun)
        {
            return @this.Map(_ => fun(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
