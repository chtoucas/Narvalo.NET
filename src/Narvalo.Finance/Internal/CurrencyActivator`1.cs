// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class CurrencyActivator<TCurrency> where TCurrency : Currency
    {
        private const string CURRENCIES_NAMESPACE = "Narvalo.Finance.Currencies";
        private const string INSTANCE_CURRENCY_FIELD = "s_Currency";

        public static TCurrency CreateInstance()
        {
            var type = typeof(TCurrency);

            object inst = null;

            // Fast-track for currencies in the Narvalo.Finance.Currencies namespace.
            if (type.Namespace == CURRENCIES_NAMESPACE)
            {
                inst = GetUniqueInstance_(type);
            }
            else
            {
                inst = CreateInstance_(type);
            }

            // Might throw an InvalidCastException.
            return (TCurrency)inst;
        }

        private static object GetUniqueInstance_(Type type)
        {
            FieldInfo fieldInfo = type.GetTypeInfo().DeclaredFields
                .Where(_ => _.Name == INSTANCE_CURRENCY_FIELD)
                .SingleOrDefault();

            if (fieldInfo == null)
            {
                // NB: This should never happen.
                throw new MissingMemberException();
            }

            return fieldInfo.GetValue(null);
        }

        // NB: Slow but only called once during the lifetime of an AppDomain.
        private static object CreateInstance_(Type type)
        {
            ConstructorInfo ctorInfo = type.GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(_ => !_.GetParameters().Any());

            if (ctorInfo == null)
            {
                throw new MissingMemberException();
            }

            return ctorInfo.Invoke(new Object[] { });
        }
    }
}
