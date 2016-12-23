// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    using Narvalo.Finance.Properties;

    internal static class CurrencyActivator<TCurrency> where TCurrency : Currency
    {
        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified type parameter.
        /// </summary>
        public static TCurrency CreateInstance()
        {
            TypeInfo typeInfo = typeof(TCurrency).GetTypeInfo();
            Contract.Assume(typeInfo != null);

            ConstructorInfo ctorInfo = typeInfo
                .DeclaredConstructors
                .FirstOrDefault(_ => !_.GetParameters().Any());

            if (ctorInfo == null)
            {
                throw new MissingMemberException(Strings.CurrencyActivator_MissingCtor);
            }

            return ctorInfo.Invoke(new Object[] { }) as TCurrency;
        }
    }
}
