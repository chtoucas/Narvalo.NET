// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public static class PredicateExtensions
    {
        public static Func<TSource, bool> Negation<TSource>(this Func<TSource, bool> @this)
        {
            return _ => !@this.Invoke(_);
        }
    }
}
