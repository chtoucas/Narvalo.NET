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
        public static Maybe<XAttribute> AttributeOrNone(this XElement @this, string name)
        {
            Require.Object(@this);

            var attr = @this.Attribute(name);
            return attr == null ? Maybe<XAttribute>.None : Maybe.Create(attr);
        }

        public static XAttribute AttributeOrThrow(this XElement @this, string name)
        {
            Require.Object(@this);

            var attr = @this.Attribute(name);
            if (attr == null) {
                throw new XmlException(
                    Format.CurrentCulture(SR.XElement_AttributeNotFoundFormat, @this.Name.LocalName, name));
            }

            return attr;
        }

        public static Maybe<XElement> ElementOrNone(this XElement @this, string name)
        {
            Require.Object(@this);

            var child = @this.Element(name);
            return child == null ? Maybe<XElement>.None : Maybe.Create(child);
        }

        public static XElement ElementOrThrow(this XElement @this, string name)
        {
            Require.Object(@this);

            var child = @this.Element(name);
            if (child == null) {
                throw new XmlException(
                    Format.CurrentCulture(SR.XElement_FirstChildNotFoundFormat, @this.Name.LocalName, name));
            }

            return child;
        }

        public static T ParseValue<T>(this XElement @this, Func<string, T> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun(@this.Value);
        }

        public static T ParseValue<T>(this XElement @this, MayFunc<string, T> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun(@this.Value).ValueOrThrow(() => new XmlException(
                Format.CurrentCulture(
                    SR.XElement_MalformedElementValueFormat,
                    @this.Name.LocalName,
                    ((IXmlLineInfo)@this).LineNumber)));
        }

        public static Maybe<T> MayParseValue<T>(this XElement @this, MayFunc<string, T> fun)
        {
            Require.Object(@this);
            Require.NotNull(fun, "fun");

            return fun(@this.Value);
        }

        public static Maybe<XElement> NextElementOrNone(this XElement @this)
        {
            Require.Object(@this);

            var nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element) {
                nextElement = nextElement.NextNode;
            }

            return Maybe.Create(nextElement as XElement);
        }

        public static XElement NextElementOrThrow(this XElement @this)
        {
            Require.Object(@this);

            var nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element) {
                nextElement = nextElement.NextNode;
            }

            if (nextElement == null) {
                throw new XmlException(
                    Format.CurrentCulture(SR.XElement_NextElementNotFoundFormat, @this.Name.LocalName));
            }

            return nextElement as XElement;
        }
    }
}
