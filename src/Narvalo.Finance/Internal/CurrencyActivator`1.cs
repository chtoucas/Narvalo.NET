// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    internal static class CurrencyActivator<TCurrency>
        where TCurrency : Currency
    {
        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified type parameter.
        /// </summary>
        /// <returns></returns>
        public static TCurrency CreateInstance()
        {
            TypeInfo typeInfo = typeof(TCurrency).GetTypeInfo();
            Contract.Assume(typeInfo != null);

            ConstructorInfo ctorInfo = typeInfo
                .DeclaredConstructors
                .FirstOrDefault(_ => !_.GetParameters().Any());

            if (ctorInfo == null)
            {
                // TODO: Throw an internal exception.
                throw new MissingMemberException();
            }

            return ctorInfo.Invoke(new Object[] { }) as TCurrency;
        }
    }
}
