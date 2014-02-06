// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;
    using Narvalo.Linq;

    /// <summary>
    /// Provides extension methods for Maybe&lt;XElement&gt;.
    /// </summary>
    public static class MaybeXElementExtensions
    {
        public static Maybe<T> BindValue<T>(this Maybe<XElement> @this, Func<string, Maybe<T>> kun)
        {
            Require.NotNull(kun, "kun");

            return @this.Bind(_ => kun.Invoke(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, "selector");

            return from _ in @this select selector.Invoke(_.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
