// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Reflection;

    public static class AssemblyUtility
    {
        public static Assembly FindEntryAssembly()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            if (assembly != null) {
                return assembly;
            }

            assembly = Assembly.GetExecutingAssembly();

            if (assembly != null) {
                return assembly;
            }

            assembly = Assembly.GetCallingAssembly();

            if (assembly != null) {
                return assembly;
            }

            return assembly;
        }
    }
}
