// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System;

    static partial class KuncExtensions
    {
        [Obsolete]
        public static Kunc<Unit, Unit> Filter(this Kunc<Unit, Unit> @this, bool predicate)
        {
            return predicate ? @this : Stubs.Noop;
        }

        [Obsolete]
        public static Kunc<TSource, Unit> Filter<TSource>(this Kunc<TSource, Unit> @this, bool predicate)
        {
            return predicate ? @this : Stubs<TSource>.Ignore;
        }
    }
}
