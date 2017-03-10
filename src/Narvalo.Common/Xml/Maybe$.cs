// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;

    using Narvalo.Applicative;

    /// <summary>
    /// Provides extension methods for <see cref="Maybe{XElement}"/> and <see cref="Maybe{XAttribute}"/>.
    /// </summary>
    // Extensions methods for Maybe<XElement>.
    public static partial class MaybeExtensions
    {
        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return from elmt in @this select selector.Invoke(elmt.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
            => from elmt in @this select elmt.Value;
    }

    // Extensions methods for Maybe<XAttribute>.
    public static partial class MaybeExtensions
    {
        public static Maybe<T> MapValue<T>(this Maybe<XAttribute> @this, Func<string, T> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return from attr in @this select selector.Invoke(attr.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XAttribute> @this)
            => from attr in @this select attr.Value;
    }
}
