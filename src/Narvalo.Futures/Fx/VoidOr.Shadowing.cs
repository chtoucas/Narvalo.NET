// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;

    public static partial class VoidOr
    {
        internal static VoidOr<IEnumerable<TError>> CollectImpl<TError>(
            this IEnumerable<VoidOr<TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<VoidOr<IEnumerable<TError>>>();

            return VoidOr.FromError<IEnumerable<TError>>(CollectAnyIterator(@this));
        }
    }
}
