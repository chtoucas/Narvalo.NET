// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu
{
    using System;
    using System.Collections.Generic;
    using Narvalo;

    public static class RandomNumber
    {
        // Cf. http://blogs.msdn.com/b/ericlippert/archive/2012/02/21/generating-random-non-uniform-data-in-c.aspx
        static Random Random_ = new Random();

        public static IEnumerable<double> UniformDistribution()
        {
            while (true) {
                yield return Random_.NextDouble();
            }
        }

        public static double CauchyQuantile_(double p)
        {
            return Math.Tan(Math.PI * (p - 0.5));
        }

        // Cf. http://en.wikipedia.org/wiki/Inverse_transform_sampling
        // var cauchy = from x in UniformDistribution() select CauchyQuantile(x);
        // int[] histogram = CreateHistogram(cauchy.Take(100000), 50, -5.0, 5.0);
        public static int[] CreateHistogram(
            IEnumerable<double> data,
            int buckets,
            double min,
            double max)
        {
            int[] results = new int[buckets];
            double multiplier = buckets / (max - min);

            foreach (double datum in data) {
                double index = (datum - min) * multiplier;

                if (0.0 <= index && index < buckets) {
                    results[(int)index] += 1;
                }
            }

            return results;
        }
    }
}
