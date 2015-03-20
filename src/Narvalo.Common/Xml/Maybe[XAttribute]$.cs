// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="Maybe{XAttribute}"/>.
    /// </summary>
    public static class MaybeXAttributeExtensions
    {
        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return from _ in @this select selector.Invoke(_.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
        {
            Require.Object(@this);

            return from _ in @this select _.Value;
        }
    }
}
