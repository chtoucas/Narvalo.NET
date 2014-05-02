// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IBuildManager
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GetType",
            Justification = "Wrapping existing framework function name.")]
        Type GetType(string typeName, bool throwOnError, bool ignoreCase);
    }
}
