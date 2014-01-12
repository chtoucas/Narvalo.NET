namespace Narvalo.Xml
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Linq;
    using Narvalo.Diagnostics;
    using Narvalo.Fx;

    public static class XAttributeExtensions
    {
        public static T ParseValue<T>(this XAttribute attr, Func<string, T> fun)
        {
            Requires.NotNull(fun, "fun");
            Requires.NotNull(attr, "@this");

            return fun(attr.Value);
        }

        public static T ParseValue<T>(this XAttribute attr, MayFunc<string, T> fun)
        {
            Requires.NotNull(fun, "fun");
            Requires.NotNull(attr, "@this");

            return fun(attr.Value).ValueOrThrow(() => new XmlException(
                String.Format(
                    CultureInfo.CurrentCulture,
                    SR.XElement_MalformedAttributeValue,
                    attr.Name.LocalName,
                    ((IXmlLineInfo)attr).LineNumber)));
        }

        public static Maybe<T> MayParseValue<T>(this XAttribute attr, MayFunc<string, T> fun)
        {
            Requires.NotNull(fun, "fun");
            Requires.NotNull(attr, "@this");

            return fun(attr.Value);
        }
    }
}
