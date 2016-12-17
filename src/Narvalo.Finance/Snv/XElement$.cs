// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="XElement"/>.
    /// </summary>
    internal static class XElementExtensions
    {
        public static XElement ElementOrThrow(this XElement @this, XName name)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<XElement>();

            XElement child = @this.Element(name);
            if (child == null) { throw new InvalidDataException(); }

            return child;
        }

        public static T AttributeOrThrow<T>(this XElement @this, XName name, Func<string, T> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<XAttribute>();

            XAttribute attr = @this.Attribute(name);
            if (attr == null) { throw new InvalidDataException(); }

            return selector.Invoke(attr.Value);
        }

        public static T AttributeOrElse<T>(this XElement @this, XName name, Func<string, T> selector, T other)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            var attr = @this.Attribute(name);
            if (attr == null) { return other; }

            return selector.Invoke(attr.Value);
        }

        public static T Value<T>(this XElement @this, Func<string, T> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(@this.Value);
        }
    }
}
