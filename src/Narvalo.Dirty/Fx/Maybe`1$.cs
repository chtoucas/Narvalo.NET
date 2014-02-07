// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using Narvalo.Linq;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="Narvalo.Fx.Maybe{T}"/>.
    /// </summary>
    public static class MaybeExtensions
    {
        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
        {
            Require.Object(@this);

            // NB: Dans un style plus axiomatique, une manière plus compliquée d'écrire les choses serait :
            // return @this.Match(_ => _, () => (T?)null);
            return @this.IsSome ? (T?)@this.Value : null;
        }

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
        {
            Require.Object(@this);

            return @this.IsSome ? @this.Value : null;
        }

        //// When

        public static Maybe<Unit> When<T>(this Maybe<T> @this, bool predicate, Func<Maybe<Unit>> kun)
        {
            Require.Object(@this);

            return @this.Bind(_ => kun.When(predicate).Invoke());
        }

        public static Maybe<Unit> When<T>(this Maybe<T> @this, bool predicate, Func<T, Maybe<Unit>> kun)
        {
            Require.Object(@this);

            return @this.Bind(kun.When(predicate));
        }

        //// Unless

        public static Maybe<Unit> Unless<T>(this Maybe<T> @this, bool predicate, Func<Maybe<Unit>> kun)
        {
            Require.Object(@this);

            return @this.When(!predicate, kun);
        }

        public static Maybe<Unit> Unless<T>(this Maybe<T> @this, bool predicate, Func<T, Maybe<Unit>> kun)
        {
            Require.Object(@this);

            return @this.When(!predicate, kun);
        }

        //// TrySet

        public static bool TrySet<T>(this Maybe<T> @this, out T value)
        {
            if (@this.IsSome) {
                value = @this.Value;
                return true;
            }
            else {
                value = default(T);
                return false;
            }
        }
    }
}
