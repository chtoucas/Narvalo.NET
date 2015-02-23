// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;

    using Narvalo.Mvp.Resolvers;

    using Compilation = System.Web.Compilation;

    public sealed class AspNetBuildManager : IBuildManager
    {
        public Type GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            return Compilation.BuildManager.GetType(typeName, throwOnError, ignoreCase);
        }
    }
}
