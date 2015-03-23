// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx.Extensions;

    /// <summary>
    /// Provides extension methods for <see cref="NameValueCollection"/> that depend on the <see cref="Maybe{T}"/> class.
    /// </summary>
    public static partial class NameValueCollectionExtensions
    {
        public static Maybe<string> MayGetSingle(this NameValueCollection @this, string name)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return from values in @this.MayGetValues(name)
                   where values.Length == 1
                   select values[0];
        }

        public static Maybe<string[]> MayGetValues(this NameValueCollection @this, string name)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<string[]>>() != null);

            return Maybe.Of(@this.GetValues(name));
        }

        public static IEnumerable<T> ParseAny<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Contract.Requires(@this != null);

            return (from @_ in @this.MayGetValues(name) select @_.MapAny(parserM)).ValueOrElse(Enumerable.Empty<T>());
        }

        public static Maybe<IEnumerable<T>> MayParseAll<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<IEnumerable<T>>>() != null);

            return @this.MayGetValues(name).Bind(@_ => @_.Map(parserM));
        }
    }
}
