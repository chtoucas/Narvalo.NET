namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Reflection;
    using Narvalo.Fx;

    static class ReflectionExtensions
    {
        public static Maybe<T> MayGetCustomAttribute<T>(
               this MemberInfo element,
               bool inherit) where T : Attribute
        {
#if NET_40
            var attr = Attribute.GetCustomAttribute(element, typeof(T), inherit);
            return Maybe.Create(attr).Map(a => (T)a);
#else
            return Maybe.Create(element.GetCustomAttribute<T>(inherit));
#endif
        }

        //public static Maybe<ConstructorInfo> MayGetDefaultConstructor(Type type)
        //{
        //    return Maybe.Create(type.GetConstructor(Type.EmptyTypes));
        //}

        // FIXME
        //public static void GetObject(Type type)
        //{
        //    // Construction du type.
        //    MayGetDefaultConstructor(type)
        //        .WhenAny(c => { c.Invoke(null /* parameters */); });
        //}

        //public static bool IsStatic(Type type)
        //{
        //    // NB: At IL level any static class is abstract and sealed.
        //    return MayGetDefaultConstructor(type).IsNone
        //        && type.IsAbstract 
        //        && type.IsSealed;
        //}
    }
}
