// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web.Compilation;

    public class BuildManagerWrapper : IBuildManager
    {
        public Type GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            return BuildManager.GetType(typeName, throwOnError, ignoreCase);
        }
    }
}
