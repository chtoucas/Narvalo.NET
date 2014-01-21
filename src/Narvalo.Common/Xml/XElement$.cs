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

        public static XAttribute AttributeOrThrow(this XElement @this, string name, XmlException exception)
        {
            Require.NotNull(exception, "exception");

            return AttributeOrThrow(@this, name, () => exception);
        }

        public static XAttribute AttributeOrThrow(this XElement @this, string name, Func<XmlException> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");

            var attr = @this.Attribute(name);
            if (attr == null) {
                throw exceptionFactory.Invoke();
            }

            return attr;
        }

        public static Maybe<XElement> ElementOrNone(this XElement @this, string name)
        {
            Require.Object(@this);

            var child = @this.Element(name);
            return child == null ? Maybe<XElement>.None : Maybe.Create(child);
        }

        public static XElement ElementOrThrow(this XElement @this, string name, XmlException exception)
        {
            Require.NotNull(exception, "exception");

            return ElementOrThrow(@this, name, () => exception);
        }

        public static XElement ElementOrThrow(this XElement @this, string name, Func<XmlException> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");

            var child = @this.Element(name);
            if (child == null) {
                throw exceptionFactory.Invoke();
            }

            return child;
        }

        public static T MapValue<T>(this XElement @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Value);
        }

        public static Maybe<T> MayParseValue<T>(this XElement @this, Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);
            Require.NotNull(parserM, "parserM");

            return parserM.Invoke(@this.Value);
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

        public static XElement NextElementOrThrow(this XElement @this, XmlException exception)
        {
            Require.NotNull(exception, "exception");

            return NextElementOrThrow(@this, () => exception);
        }

        public static XElement NextElementOrThrow(this XElement @this, Func<XmlException> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");

            var nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element) {
                nextElement = nextElement.NextNode;
            }

            if (nextElement == null) {
                throw exceptionFactory.Invoke();
            }

            return nextElement as XElement;
        }
    }
}
