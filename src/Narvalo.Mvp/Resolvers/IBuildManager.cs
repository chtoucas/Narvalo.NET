// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IBuildManager
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GetType",
            Justification = "Wraps an existing framework function name.")]
        Type GetType(string typeName, bool throwOnError, bool ignoreCase);
    }
}
