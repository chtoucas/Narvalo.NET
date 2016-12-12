// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial interface IBuildManager
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GetType",
            Justification = "Wraps an existing framework function name.")]
        Type GetType(string typeName, bool throwOnError, bool ignoreCase);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IBuildManagerContract))]
    public partial interface IBuildManager { }

    [ContractClassFor(typeof(IBuildManager))]
    internal abstract class IBuildManagerContract : IBuildManager
    {
        Type IBuildManager.GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            Requires(typeName != null);
            // NB: We do not enforce null return value.

            return default(Type);
        }
    }
}

#endif
