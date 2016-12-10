// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;

    using Narvalo.Fx;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Provides extension methods for <see cref="XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        public static T Value<T>(this XAttribute @this, Func<string, T> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(@this.Value);
        }

        public static Maybe<XAttribute> NextAttributeOrNone(this XAttribute @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(@this.NextAttribute);
        }

        public static XAttribute NextAttributeOrThrow(this XAttribute @this, Exception exception)
        {
            Require.NotNull(exception, nameof(exception));
            Expect.NotNull(@this);
            Ensures(Result<XAttribute>() != null);

            return NextAttributeOrThrow(@this, () => exception);
        }

        public static XAttribute NextAttributeOrThrow(this XAttribute @this, Func<Exception> exceptionFactory)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));
            Ensures(Result<XAttribute>() != null);

            XAttribute attr = @this.NextAttribute;
            if (attr == null)
            {
                throw exceptionFactory.Invoke();
            }

            return attr;
        }

        public static Maybe<XAttribute> PreviousAttributeOrNone(this XAttribute @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(@this.PreviousAttribute);
        }

        public static XAttribute PreviousAttributeOrThrow(this XAttribute @this, Exception exception)
        {
            Require.NotNull(exception, nameof(exception));
            Expect.NotNull(@this);
            Ensures(Result<XAttribute>() != null);

            return PreviousAttributeOrThrow(@this, () => exception);
        }

        public static XAttribute PreviousAttributeOrThrow(this XAttribute @this, Func<Exception> exceptionFactory)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));
            Ensures(Result<XAttribute>() != null);

            XAttribute attr = @this.PreviousAttribute;
            if (attr == null)
            {
                throw exceptionFactory.Invoke();
            }

            return attr as XAttribute;
        }
    }
}
