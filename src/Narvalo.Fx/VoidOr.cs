// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="VoidOr{TError}"/>.
    /// </summary>
    public partial class VoidOr
    {
        public static void ThrowIfError(this VoidOr<ExceptionDispatchInfo> @this)
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                @this.Error.Throw();
            }
        }

        public static void ThrowIfError<TException>(this VoidOr<TException> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                throw @this.Error;
            }
        }
    }
}
