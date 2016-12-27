// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Currencies
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    internal static class CurrencyActivator
    {
        // NB: On full .NET, there is a much faster alternative, namely:
        //   Activator.CreateInstance(typeof(TCurrency), nonPublic: true) as TCurrency;
        public static TCurrency CreateInstance<TCurrency>() where TCurrency : CurrencyUnit<TCurrency>
        {
            var typeInfo = typeof(TCurrency).GetTypeInfo();
            Contract.Assume(typeInfo != null);

            // Unsafe, we assume that there is a single constructor, the default one.
            // Nevertheless, it should not be possible to create a currency unit outside this assembly.
            return typeInfo.DeclaredConstructors.Single().Invoke(null) as TCurrency;
        }
    }
}
