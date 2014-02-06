namespace Narvalo.Benchmark.Internal
{
    using System;
    using System.Reflection;
    using Narvalo.Fx;
    using Narvalo.Linq;

    static class BenchmarkFactory
    {
        //public static Benchmark Create(MethodInfo method)
        //{
        //    return MayCreate(method)
        //        .ValueOrThrow(
        //            () => {
        //                var message = String.Format(
        //                    CultureInfo.CurrentCulture,
        //                    SR.MissingBenchmarkAttribute,
        //                    method.Name);
        //                return new BenchException(message);
        //            }
        //        );
        //}

        public static Maybe<Benchmark> MayCreate(MethodInfo method)
        {
            return from attr in method.MayGetCustomAttribute<BenchmarkAttribute>(false /* inherit */)
                   select new Benchmark(
                       GetName(method, attr),
                       ActionFactory.Create(method),
                       attr.Iterations);
        }

        #region Membres privés.

        static string GetName(MethodInfo method, BenchmarkAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? method.Name : attr.DisplayName;
        }

        #endregion
    }
}
