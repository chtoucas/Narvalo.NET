namespace Narvalo.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Xml.Linq.XElement"/>.
    /// </summary>
    public static class XElementExtensions
    {
        [Obsolete]
        public static T ParseValue<T>(this XElement @this, Func<string, Maybe<T>> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun.Invoke(@this.Value).ValueOrThrow(() => new XmlException(
                Format.CurrentCulture(
                    SR.XElement_MalformedElementValueFormat,
                    @this.Name.LocalName,
                    ((IXmlLineInfo)@this).LineNumber)));
        }
    }
}
