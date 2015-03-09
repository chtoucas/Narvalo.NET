// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    internal static class GuardExtensions
    {
        public static bool IsChainable(this IGuard guard)
        {
            Require.NotNull(guard, "guard");

            return guard.Multiplicity == 1;
        }
    }
}
