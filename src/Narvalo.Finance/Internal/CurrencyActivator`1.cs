// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class CurrencyActivator<TCurrency> where TCurrency : Currency
    {
        private const string CURRENCIES_NAMESPACE = "Narvalo.Finance.Currencies";
        private const string INSTANCE_CURRENCY_FIELD = "s_Currency";

        public static TCurrency CreateInstance()
        {
            var type = typeof(TCurrency);

            // Fast-track for currencies in the Narvalo.Finance.Currencies namespace.
            if (type.Namespace == CURRENCIES_NAMESPACE)
            {
                return GetUniqueInstance(type);
            }

            var inst = CreateInstance(type);

            if (inst == null)
            {
                // TODO: Throw a more meaningful exception.
                throw new Exception();
            }

            return inst;
        }

        internal static TCurrency GetUniqueInstance(Type type)
        {
            // var fieldInfo = type.GetField(INSTANCE_CURRENCY_FIELD, BindingFlags.NonPublic | BindingFlags.Static);
            var typeInfo = type.GetTypeInfo();
            var fieldInfo = (from _ in typeInfo.DeclaredFields
                             where _.Name == INSTANCE_CURRENCY_FIELD
                             select _).Single();

            var fieldValue = fieldInfo.GetValue(null);

            return (TCurrency)fieldValue;
        }

        internal static TCurrency CreateInstance(Type type)
        {
            // var ctorInfo = type.GetConstructor(
            //     BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
            //     null,
            //     Type.EmptyTypes,
            //     null);
            var ctorInfo = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(_ => !_.GetParameters().Any());

            if (ctorInfo == null)
            {
                return null;
            }

            throw new NotImplementedException();

            ////var method = new DynamicMethod("NewTCurrency", type, Type.EmptyTypes, type, skipVisibility: true);

            ////var il = method.GetILGenerator();
            ////il.Emit(OpCodes.Newobj, ctorInfo);
            ////il.Emit(OpCodes.Ret);

            ////var ctor = (Func<TCurrency>)method.CreateDelegate(typeof(Func<TCurrency>));

            ////return ctor.Invoke();
        }
    }
}
