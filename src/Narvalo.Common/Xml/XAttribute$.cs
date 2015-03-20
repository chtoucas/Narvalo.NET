// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Xml.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        public static T Value<T>(this XAttribute @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Value);
        }

        public static Maybe<XAttribute> NextAttributeOrNone(this XAttribute @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<XAttribute>>() != null);

            return Maybe.Create(@this.NextAttribute);
        }

        public static XAttribute NextAttributeOrThrow(this XAttribute @this, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<XAttribute>() != null);

            return NextAttributeOrThrow(@this, () => exception);
        }

        public static XAttribute NextAttributeOrThrow(this XAttribute @this, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<XAttribute>() != null);

            XAttribute attr = @this.NextAttribute;
            if (attr == null)
            {
                throw exceptionFactory.Invoke();
            }

            return attr;
        }

        public static Maybe<XAttribute> PreviousAttributeOrNone(this XAttribute @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<XAttribute>>() != null);

            return Maybe.Create(@this.PreviousAttribute);
        }

        public static XAttribute PreviousAttributeOrThrow(this XAttribute @this, Exception exception)
        {
            Require.NotNull(exception, "exception");
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<XAttribute>() != null);

            return PreviousAttributeOrThrow(@this, () => exception);
        }

        public static XAttribute PreviousAttributeOrThrow(this XAttribute @this, Func<Exception> exceptionFactory)
        {
            Require.Object(@this);
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<XAttribute>() != null);

            XAttribute attr = @this.PreviousAttribute;
            if (attr == null)
            {
                throw exceptionFactory.Invoke();
            }

            return attr as XAttribute;
        }
    }
}
