// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    internal static class TypeUtility
    {
        [Pure]
        public static bool HasFlagsAttribute(Type type)
        {
            Contract.Requires(type != null);

            var attribute = type.GetCustomAttribute<FlagsAttribute>(inherit: false);

            return attribute != null;
        }
    }
}
