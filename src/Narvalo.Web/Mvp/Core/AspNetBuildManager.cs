// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp.Core
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
