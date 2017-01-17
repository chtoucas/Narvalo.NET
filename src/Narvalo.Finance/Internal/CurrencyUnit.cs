// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using Narvalo.Finance.Generic;

    internal partial class CurrencyUnit
    {
        private const string SINGLETON_PROPERTY_NAME = "Unit";

        // NB: On full .NET, there is a much faster alternative, namely:
        //   Activator.CreateInstance(typeof(TCurrency), nonPublic: true) as TCurrency;
        // Nevertheless, calling the constructor somehow defeats the idea of a unit being a singleton.
        public static TCurrency OfType<TCurrency>() where TCurrency : Currency<TCurrency>
        {
            Warrant.NotNull<TCurrency>();

            var typeInfo = typeof(TCurrency).GetTypeInfo();
            Contract.Assume(typeInfo != null);

            // We expect that all built-in currency units defines this property.
            var property = typeInfo.GetDeclaredProperty(SINGLETON_PROPERTY_NAME);

            return property?.GetValue(null) as TCurrency;
        }
    }
}
