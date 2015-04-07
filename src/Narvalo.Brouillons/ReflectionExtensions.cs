// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Reflection;

    using Narvalo.Fx;

    internal static class ReflectionExtensions
    {
        public static Maybe<TAttribute> MayGetCustomAttribute<TAttribute>(
               this Type @this,
               bool inherit) where TAttribute : Attribute
        {
            return Maybe.Of(@this.GetCustomAttribute<TAttribute>(inherit));

            //#if NET_35
            //#else
            //            var attr = Attribute.GetCustomAttribute(element, typeof(T), inherit);
            //            return Maybe.Of(attr).Select(a => (T)a);
            //#endif
        }

        ///// <summary>
        ///// Returns the <see cref="System.Reflection.TypeInfo"/> representation of the specified type.
        ///// Workaround for the fact that IntrospectionExtensions.GetTypeInfo() does not have any contract attached.
        ///// </summary>
        ///// <remarks>This method MUST remain public in order to be used inside CC preconditions.</remarks>
        ///// <param name="type">The type to convert.</param>
        ///// <returns>The converted object.</returns>
        //[Pure]
        //public static TypeInfo GetTypeInfo(this Type @this)
        //{
        //    Contract.Ensures(Contract.Result<TypeInfo>() != null);

        //    // NB: The properties Type.IsEnum and Type.IsValueType do not exist in the context of PCL.
        //    return @this.GetTypeInfo().AssumeNotNull();
        //}

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
