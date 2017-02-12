// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public partial class VoidOr
    {
        public static void ThrowIfError<TException>(VoidOr<TException> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsMessage)
            {
                throw @this.Message;
            }
        }
    }
}
