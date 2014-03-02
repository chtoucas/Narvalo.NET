namespace Narvalo.Benchmark.Internal
{
    using System;
    using System.Reflection;
    using Narvalo.Fx;

    static class BenchComparativeFactory
    {
        //public static BenchComparative Create(MethodInfo method)
        //{
        //    return MayCreate(method)
        //        .ValueOrThrow(GetExceptionThunk(method.Name));
        //}

        // FIXME: Theory.
        //public static BenchComparative Create<T>(MethodInfo method, T value)
        //{
        //    return MayCreate(method, value)
        //        .ValueOrThrow(GetExceptionThunk(method.Name));
        //}

        public static Maybe<BenchComparative> MayCreate(MethodInfo method)
        {
            return from attr in MayGetAttribute(method)
                   select new BenchComparative(
                       GetName(method, attr),
                       ActionFactory.Create(method));
        }

        // FIXME: Theory.
        public static Maybe<BenchComparative> MayCreate<T>(MethodInfo method, T value)
        {
            return from attr in MayGetAttribute(method)
                   select new BenchComparative(
                       GetName(method, attr),
                       () => ActionFactory.Create<T>(method).Invoke(value));
        }

        #region Membres privés.

        //static Func<BenchException> GetExceptionThunk(string name)
        //{
        //    return () => {
        //        var message = String.Format(
        //            CultureInfo.CurrentCulture,
        //            SR.MissingBenchComparativeAttribute,
        //            name);
        //        return new BenchException(message);
        //    };
        //}

        static string GetName(MethodInfo method, BenchComparativeAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? method.Name : attr.DisplayName;
        }

        static Maybe<BenchComparativeAttribute> MayGetAttribute(MethodInfo method)
        {
            return method
                .MayGetCustomAttribute<BenchComparativeAttribute>(false /* inherit */);
        }

        #endregion
    }
}
