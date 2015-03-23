﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Reflection;

    using Narvalo.Fx;

    internal static class ReflectionExtensions
    {
        public static Maybe<T> MayGetCustomAttribute<T>(
               this MemberInfo element,
               bool inherit) where T : Attribute
        {
#if NET_35
            return Maybe.Create(element.GetCustomAttribute<T>(inherit));
#else
            var attr = Attribute.GetCustomAttribute(element, typeof(T), inherit);
            return Maybe.Of(attr).Select(a => (T)a);
#endif
        }

        ////public static Maybe<ConstructorInfo> MayGetDefaultConstructor(Type type)
        ////{
        ////    return Maybe.Create(type.GetConstructor(Type.EmptyTypes));
        ////}

        ////public static void GetObject(Type type)
        ////{
        ////    // Construction du type.
        ////    MayGetDefaultConstructor(type)
        ////        .WhenAny(c => { c.Invoke(null /* parameters */); });
        ////}

        ////public static bool IsStatic(Type type)
        ////{
        ////    // NB: At IL level any static class is abstract and sealed.
        ////    return MayGetDefaultConstructor(type).IsNone
        ////        && type.IsAbstract 
        ////        && type.IsSealed;
        ////}
    }
}
