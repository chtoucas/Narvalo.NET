// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    internal static class ReflectionExtensions
    {
        [Pure]
        public static bool HasFlagsAttribute(this MemberInfo @this)
        {
            Contract.Requires(@this != null);

            var attribute = @this.GetCustomAttribute<FlagsAttribute>(inherit: false);

            return attribute != null;
        }
    }
}
