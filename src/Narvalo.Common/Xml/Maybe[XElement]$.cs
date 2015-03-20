// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="Maybe{XElement}"/>.
    /// </summary>
    public static class MaybeXElementExtensions
    {
        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return from _ in @this select selector.Invoke(_.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            Require.Object(@this);

            return from _ in @this select _.Value;
        }
    }
}
