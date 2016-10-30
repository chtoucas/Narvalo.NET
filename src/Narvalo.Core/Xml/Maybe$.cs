// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="Maybe{XElement}"/>.
    /// </summary>
    public static partial class MaybeExtensions
    {
        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return from _ in @this select selector.Invoke(_.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            return from _ in @this select _.Value;
        }
    }

    /// <content>
    /// Provides extension methods for <see cref="Maybe{XAttribute}"/>.
    /// </content>
    public static partial class MaybeExtensions
    {
        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return from _ in @this select selector.Invoke(_.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
        {
            return from _ in @this select _.Value;
        }
    }
}
