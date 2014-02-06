// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="System.Collections.Specialized.NameValueCollection"/>.
    /// </summary>
    public static partial class NameValueCollectionExtensions
    {
        public static T? ParseValue<T>(this NameValueCollection @this, string name, Func<string, T?> parserM)
            where T : struct
        {
            Require.Object(@this);
            Require.NotNull(parserM, "parserM");

            return (from _ in @this.MayGetSingle(name) select parserM.Invoke(_)).ValueOrDefault();
        }
    }
}
