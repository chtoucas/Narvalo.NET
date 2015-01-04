// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Xml.Linq.XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        public static T Select<T>(this XAttribute @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Value);
        }

        public static Maybe<XAttribute> NextAttributeOrNone(this XAttribute @this)
        {
            Require.Object(@this);

            return Maybe.Create(@this.NextAttribute);
        }

        public static XAttribute NextAttributeOrThrow(this XAttribute @this, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Contract.Requires(@this != null);

            return NextAttributeOrThrow(@this, () => exception);
        }

        public static XAttribute NextAttributeOrThrow(this XAttribute @this, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");

            var attr = @this.NextAttribute;
            if (attr == null) {
                throw exceptionFactory.Invoke();
            }

            return attr as XAttribute;
        }

        public static Maybe<XAttribute> PreviousAttributeOrNone(this XAttribute @this)
        {
            Require.Object(@this);

            return Maybe.Create(@this.PreviousAttribute);
        }

        public static XAttribute PreviousAttributeOrThrow(this XAttribute @this, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Contract.Requires(@this != null);

            return PreviousAttributeOrThrow(@this, () => exception);
        }

        public static XAttribute PreviousAttributeOrThrow(this XAttribute @this, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");

            var attr = @this.PreviousAttribute;
            if (attr == null) {
                throw exceptionFactory.Invoke();
            }

            return attr as XAttribute;
        }
    }
}
