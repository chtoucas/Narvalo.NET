// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Maybe{S}"/>.
    /// </summary>
    public static partial class Maybe { }

    // Provides extension methods for Maybe<T?> or Maybe<T> where T is a struct.
    public static partial class Maybe
    {
        // Conversion from T? to Maybe<T>.
        // NB: This method makes it impossible to create a Maybe<T?> **directly**.
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;

        // Conversion from Maybe<T?> to  Maybe<T>.
        public static Maybe<T> Squash<T>(this Maybe<T?> @this) where T : struct
            // NB: When IsSome is true, Value.HasValue is always true,
            // therefore we can safely access Value.Value. Indeed, the only way
            // to construct a maybe is via "η" which returns "none" if the
            // input is null (input.HasValue is false).
            => @this.IsSome
            ? Maybe.Of(@this.Value.Value)
            : Maybe<T>.None;

        // Conversion from Maybe<T> to T?.
        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;

        // Conversion from Maybe<T?> to T?.
        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
#if DEBUG
            // We have to be careful in Debug mode, the access to Value is
            // protected by a Debug.Assert.
            => @this.IsSome ? @this.Value : null;
#else
            // If the object is "none", Value is default(T?) ie null.
            => @this.Value;
#endif
    }

    public static partial class Maybe
    {
        public static IEnumerable<T> ValueOrEmpty<T>(this Maybe<IEnumerable<T>> @this)
            => @this.IsSome ? @this.Value : Enumerable.Empty<T>();
    }
}
