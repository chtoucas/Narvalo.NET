// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Linq;
    using System.Reflection;

    public sealed partial class /*Default*/BuildManager : IBuildManager
    {
        private readonly Assembly[] _assemblies;

        public BuildManager(Assembly assembly) : this(new[] { assembly }) { }

        public BuildManager(Assembly[] assemblies)
        {
            Require.NotNull(assemblies, nameof(assemblies));

            _assemblies = assemblies;
        }

        public Type GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            return _assemblies
                .Select(_ => _.GetType(typeName, throwOnError, ignoreCase))
                .FirstOrDefault();
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Resolvers
{
    using System.Diagnostics.Contracts;

    public sealed partial class BuildManager
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_assemblies != null);
        }
    }
}

#endif
