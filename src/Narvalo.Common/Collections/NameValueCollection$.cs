// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="System.Collections.Specialized.NameValueCollection"/>.
    /// </summary>
    public static partial class NameValueCollectionExtensions
    {
        public static Maybe<string> MayGetSingle(this NameValueCollection @this, string name)
        {
            Require.Object(@this);

            return from values in @this.MayGetValues(name)
                   where values.Length == 1
                   select values[0];
        }

        public static Maybe<string[]> MayGetValues(this NameValueCollection @this, string name)
        {
            Require.Object(@this);

            return Maybe.Create(@this.GetValues(name));
        }

        public static IEnumerable<T> ParseAny<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            return (from @_ in @this.MayGetValues(name) select @_.ConvertAny(parserM)).ValueOrElse(Enumerable.Empty<T>());
        }

        public static Maybe<IEnumerable<T>> MayParseAll<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            return @this.MayGetValues(name).Bind(@_ => @_.Map(parserM));
        }
    }
}
