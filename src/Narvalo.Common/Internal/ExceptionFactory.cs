// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static class ExceptionFactory
    {
        internal static ArgumentNullException ArgumentNull(string parameterName)
        {
            return new ArgumentNullException(parameterName, Format.CurrentCulture(SR.ExceptionFactory_ArgumentNullFormat, parameterName));
        }
    }
}
