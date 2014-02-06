// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Linq;

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

        public static Maybe<T[]> MayParseAny<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            return from values in @this.MayGetValues(name)
                   let result = values.SelectAny(parserM).ToArray()
                   where result.Length > 0
                   select result;
        }

        public static Maybe<T[]> MayParseAll<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            var list = @this.MayGetValues(name).Bind(@_ => @_.MayConvertAll(parserM));

            return from values in list select values.ToArray();
        }

        public static Maybe<T> MayParseSingle<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            return @this.MayGetSingle(name).Bind(parserM);
        }
    }
}
