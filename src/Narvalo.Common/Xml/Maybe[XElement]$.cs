// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.Contracts;
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
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return from _ in @this select selector.Invoke(_.Value);
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return from _ in @this select _.Value;
        }
    }
}
