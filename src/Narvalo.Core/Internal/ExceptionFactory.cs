// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.Contracts;

    static class ExceptionFactory
    {
        internal static ArgumentNullException ArgumentNull(string parameterName)
        {
            Contract.Requires(parameterName != null);

            return new ArgumentNullException(
                parameterName,
                Format.CurrentCulture(Strings_Core.ExceptionFactory_ArgumentNullFormat, parameterName));
        }
    }
}
