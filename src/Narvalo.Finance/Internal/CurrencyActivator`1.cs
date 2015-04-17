// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class CurrencyActivator<TCurrency>
        where TCurrency : Currency, new()
    {
        private const string CURRENCIES_NAMESPACE = "Narvalo.Finance.Currencies";
        private const string INSTANCE_CURRENCY_FIELD = "s_Currency";

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified type parameter.
        /// </summary>
        /// <returns></returns>
        public static TCurrency CreateInstance()
        {
            var type = typeof(TCurrency);

            // Handle the special case of currencies in the Narvalo.Finance.Currencies namespace.
            if (type.Namespace == CURRENCIES_NAMESPACE)
            {
                return GetUniqueInstance_(type);
            }
            else
            {
                return new TCurrency();
            }
        }

        private static TCurrency GetUniqueInstance_(Type type)
        {
            FieldInfo fieldInfo = type.GetTypeInfo().DeclaredFields
                .Where(_ => _.Name == INSTANCE_CURRENCY_FIELD)
                .SingleOrDefault();

            if (fieldInfo == null)
            {
                // NB: This should never happen.
                throw new MissingMemberException();
            }

            var inst = fieldInfo.GetValue(null);

            // Could throw an InvalidCastException but should never happen.
            return (TCurrency)inst;
        }
    }
}
