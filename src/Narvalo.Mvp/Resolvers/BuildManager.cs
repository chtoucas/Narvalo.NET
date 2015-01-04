// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Linq;
    using System.Reflection;

    public sealed class /*Default*/BuildManager : IBuildManager
    {
        readonly Assembly[] _assemblies;

        public BuildManager(Assembly assembly) : this(new[] { assembly }) { }

        public BuildManager(Assembly[] assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

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
