// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Fx.More;

    /// <summary>
    /// Provides extension methods for <see cref="NameValueCollection"/> that depend on the <see cref="Maybe{T}"/> class.
    /// </summary>
    public static partial class NameValueCollectionExtensions
    {
        public static Maybe<string> MayGetSingle(this NameValueCollection @this, string name)
        {
            Expect.NotNull(@this);

            return from values in @this.MayGetValues(name)
                   where values.Length == 1
                   select values[0];
        }

        public static Maybe<string[]> MayGetValues(this NameValueCollection @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(@this.GetValues(name));
        }

        public static IEnumerable<T> ParseAny<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Expect.NotNull(@this);
            Warrant.NotNull<IEnumerable<T>>();

            return (from @_ in @this.MayGetValues(name) select @_.SelectAny(parserM)).ValueOrElse(Enumerable.Empty<T>());
        }

        public static Maybe<IEnumerable<T>> MayParseAll<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Expect.NotNull(@this);

            return @this.MayGetValues(name).Bind(@_ => @_.ForEach(parserM));
        }
    }
}
