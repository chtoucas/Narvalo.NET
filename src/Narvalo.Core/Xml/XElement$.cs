// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.Contracts;
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
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Value);
        }

        public static Maybe<XAttribute> AttributeOrNone(this XElement @this, XName name)
        {
            Require.Object(@this);

            return Maybe.Of(@this.Attribute(name));
        }

        public static XAttribute AttributeOrThrow(this XElement @this, XName name, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Acknowledge.Object(@this);
            Contract.Ensures(Contract.Result<XAttribute>() != null);

            return AttributeOrThrow(@this, name, () => exception);
        }

        public static XAttribute AttributeOrThrow(this XElement @this, XName name, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<XAttribute>() != null);

            XAttribute attr = @this.Attribute(name);
            if (attr == null)
            {
                throw exceptionFactory.Invoke();
            }

            return attr;
        }

        public static Maybe<XElement> ElementOrNone(this XElement @this, XName name)
        {
            Require.Object(@this);

            return Maybe.Of(@this.Element(name));
        }

        public static XElement ElementOrThrow(this XElement @this, XName name, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Acknowledge.Object(@this);
            Contract.Ensures(Contract.Result<XElement>() != null);

            return ElementOrThrow(@this, name, () => exception);
        }

        public static XElement ElementOrThrow(this XElement @this, XName name, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<XElement>() != null);

            XElement child = @this.Element(name);
            if (child == null)
            {
                throw exceptionFactory.Invoke();
            }

            return child;
        }

        public static XElement NextElement(this XElement @this)
        {
            Require.Object(@this);

            XNode nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element)
            {
                nextElement = nextElement.NextNode;
            }

            return nextElement as XElement;
        }

        public static Maybe<XElement> NextElementOrNone(this XElement @this)
        {
            Require.Object(@this);

            return Maybe.Of(NextElement(@this));
        }

        public static XElement NextElementOrThrow(this XElement @this, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Acknowledge.Object(@this);
            Contract.Ensures(Contract.Result<XElement>() != null);

            return NextElementOrThrow(@this, () => exception);
        }

        public static XElement NextElementOrThrow(this XElement @this, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<XElement>() != null);

            XNode nextElement = @this.NextNode;
            while (nextElement != null && nextElement.NodeType != XmlNodeType.Element)
            {
                nextElement = nextElement.NextNode;
            }

            var retval = nextElement as XElement;

            if (retval == null)
            {
                throw exceptionFactory.Invoke();
            }

            return retval;
        }
    }
}
