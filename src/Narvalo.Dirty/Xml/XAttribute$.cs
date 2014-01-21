namespace Narvalo.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Xml.Linq.XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        [Obsolete]
        public static T ParseValue<T>(this XAttribute @this, Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);
            Require.NotNull(parserM, "parserM");

            return parserM.Invoke(@this.Value).ValueOrThrow(() => new XmlException(
                Format.CurrentCulture(
                    SR.XElement_MalformedAttributeValueFormat,
                    @this.Name.LocalName,
                    ((IXmlLineInfo)@this).LineNumber)));
        }
    }
}
