namespace Narvalo.Xml
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Xml.Linq.XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        public static T ParseValue<T>(this XAttribute @this, Func<string, T> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun(@this.Value);
        }

        public static T ParseValue<T>(this XAttribute @this, MayFunc<string, T> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun(@this.Value).ValueOrThrow(() => new XmlException(
                String.Format(
                    CultureInfo.CurrentCulture,
                    SR.XElement_MalformedAttributeValueFormat,
                    @this.Name.LocalName,
                    ((IXmlLineInfo)@this).LineNumber)));
        }

        public static Maybe<T> MayParseValue<T>(this XAttribute @this, MayFunc<string, T> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun(@this.Value);
        }
    }
}
