// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for Maybe&lt;XAttribute&gt;.
    /// </summary>
    public static class MaybeXAttributeExtensions
    {
        public static Maybe<T> BindValue<T>(this Maybe<XAttribute> @this, Func<string, Maybe<T>> kun)
        {
            Require.NotNull(kun, "kun");

            return @this.Bind(_ => kun.Invoke(_.Value));
        }

        public static Maybe<T> SelectValue<T>(this Maybe<XAttribute> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, "selector");

            return @this.Select(_ => selector.Invoke(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
        {
            return from _ in @this select _.Value;
        }
    }
}
