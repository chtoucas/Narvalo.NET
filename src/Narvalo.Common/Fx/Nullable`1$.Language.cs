// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class NullableExtensions
    {
        //// Zip

        public static TResult? Zip<TFirst, TSecond, TResult>(
            this TFirst? @this,
            TSecond? second,
            Func<TFirst, TSecond, TResult> resultSelector)
            where TFirst : struct
            where TSecond : struct
            where TResult : struct
        {
            return @this.HasValue && second.HasValue ? (TResult?)resultSelector.Invoke(@this.Value, second.Value) : null;
        }

        //// Run

        public static TSource? Run<TSource>(this TSource? @this, Action<TSource> action)
            where TSource : struct
        {
            return OnValue(@this, action);
        }

        //// Then (C# has direct support for this)

        //// Coalesce (C# has direct support for this)

        //// Otherwise (C# has direct support for this)

        //// OnZero

        public static TSource? OnZero<TSource>(this TSource? @this, Action action)
            where TSource : struct
        {
            return OnNull(@this, action);
        }
    }
}
