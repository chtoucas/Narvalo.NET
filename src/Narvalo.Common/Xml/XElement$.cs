// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="XElement"/>.
    /// </summary>
    public static class XElementExtensions
    {
        public static T Value<T>(this XElement @this, Func<string, T> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(@this.Value);
        }

        public static Maybe<XAttribute> AttributeOrNone(this XElement @this, XName name)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(@this.Attribute(name));
        }

        public static XAttribute AttributeOrThrow(this XElement @this, XName name, Exception exception)
        {
            Require.NotNull(exception, nameof(exception));

            return AttributeOrThrow(@this, name, () => exception);
        }

        public static XAttribute AttributeOrThrow(this XElement @this, XName name, Func<Exception> exceptionFactory)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            XAttribute attr = @this.Attribute(name);
            if (attr == null)
            {
                throw exceptionFactory.Invoke();
            }

            return attr;
        }

        public static Maybe<XElement> ElementOrNone(this XElement @this, XName name)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(@this.Element(name));
        }

        public static XElement ElementOrThrow(this XElement @this, XName name, Exception exception)
        {
            Require.NotNull(exception, nameof(exception));

            return ElementOrThrow(@this, name, () => exception);
        }

        public static XElement ElementOrThrow(this XElement @this, XName name, Func<Exception> exceptionFactory)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            XElement child = @this.Element(name);
            if (child == null)
            {
                throw exceptionFactory.Invoke();
            }

            return child;
        }

        public static XElement NextElement(this XElement @this)
        {
            Require.NotNull(@this, nameof(@this));

            XNode nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element)
            {
                nextElement = nextElement.NextNode;
            }

            return nextElement as XElement;
        }

        public static Maybe<XElement> NextElementOrNone(this XElement @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(NextElement(@this));
        }

        public static XElement NextElementOrThrow(this XElement @this, Exception exception)
        {
            Require.NotNull(exception, nameof(exception));

            return NextElementOrThrow(@this, () => exception);
        }

        public static XElement NextElementOrThrow(this XElement @this, Func<Exception> exceptionFactory)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            XNode nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element)
            {
                nextElement = nextElement.NextNode;
            }

            var elmt = nextElement as XElement;

            if (elmt == null)
            {
                throw exceptionFactory.Invoke();
            }

            return elmt;
        }
    }
}
