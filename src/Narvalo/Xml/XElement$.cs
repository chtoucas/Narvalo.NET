namespace Narvalo.Xml
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Linq;
    using Narvalo.Fx;
    using Narvalo.Resources;

    public static class XElementExtensions
    {
        public static Maybe<XAttribute> AttributeOrNone(this XElement element, string name)
        {
            Requires.Object(element);

            var attr = element.Attribute(name);
            return attr == null ? Maybe<XAttribute>.None : Maybe.Create(attr);
        }

        public static XAttribute AttributeOrThrow(this XElement element, string name)
        {
            Requires.Object(element);

            var attr = element.Attribute(name);
            if (attr == null) {
                throw new XmlException(String.Format(
                    CultureInfo.CurrentCulture,
                    SR.XElement_AttributeNotFound,
                    element.Name.LocalName,
                    name));
            }
            return attr;
        }

        public static Maybe<XElement> ElementOrNone(this XElement element, string name)
        {
            Requires.Object(element);

            var child = element.Element(name);
            return child == null ? Maybe<XElement>.None : Maybe.Create(child);
        }

        public static XElement ElementOrThrow(this XElement element, string name)
        {
            Requires.Object(element);

            var child = element.Element(name);
            if (child == null) {
                throw new XmlException(String.Format(
                    CultureInfo.CurrentCulture,
                    SR.XElement_FirstChildNotFound,
                    element.Name.LocalName,
                    name));
            }
            return child;
        }

        public static T ParseValue<T>(this XElement element, Func<string, T> fun)
        {
            Requires.Object(element);
            Requires.NotNull(fun, "fun");

            return fun(element.Value);
        }

        public static T ParseValue<T>(this XElement element, MayFunc<string, T> fun)
        {
            Requires.Object(element);
            Requires.NotNull(fun, "fun");

            return fun(element.Value).ValueOrThrow(() => new XmlException(
                String.Format(
                    CultureInfo.CurrentCulture,
                    SR.XElement_MalformedElementValue,
                    element.Name.LocalName,
                    ((IXmlLineInfo)element).LineNumber)));
        }

        public static Maybe<T> MayParseValue<T>(this XElement element, MayFunc<string, T> fun)
        {
            Requires.Object(element);
            Requires.NotNull(fun, "fun");

            return fun(element.Value);
        }

        public static Maybe<XElement> NextElementOrNone(this XElement element)
        {
            Requires.Object(element);

            var nextElement = element.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element) {
                nextElement = nextElement.NextNode;
            }

            return Maybe.Create(nextElement as XElement);
        }

        public static XElement NextElementOrThrow(this XElement element)
        {
            Requires.Object(element);

            var nextElement = element.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element) {
                nextElement = nextElement.NextNode;
            }
            if (nextElement == null) {
                throw new XmlException(String.Format(
                    CultureInfo.CurrentCulture,
                    SR.XElement_NextElementNotFound,
                    element.Name.LocalName));
            }

            return nextElement as XElement;
        }
    }
}
