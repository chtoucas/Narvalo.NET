// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.Diagnostics.Benchmarking;
    using Narvalo.Internal;

    public static class Int64EncoderBenchmarks
    {
        [Benchmark]
        public static void ToBase58String_Zero()
        {
            Int64Encoder.ToBase58String(0L).Consume();
        }

        [Benchmark]
        public static void ToBase58String_RandomValue()
        {
            Int64Encoder.ToBase58String(3471391110L).Consume();
        }

        [Benchmark]
        public static void ToBase58String_MaxValue()
        {
            Int64Encoder.ToBase58String(Int64.MaxValue).Consume();
        }

        [Benchmark]
        public static void FromBase58String_MaxValue()
        {
            Int64Encoder.FromBase58String("NQm6nKp8qFC").Consume();
        }

        [Benchmark]
        public static void ToFlickrBase58String_Zero()
        {
            Int64Encoder.ToFlickrBase58String(0L).Consume();
        }

        [Benchmark]
        public static void ToFlickrBase58String_RandomValue()
        {
            Int64Encoder.ToFlickrBase58String(3471391110L).Consume();
        }

        [Benchmark]
        public static void ToFlickrBase58String_MaxValue()
        {
            Int64Encoder.ToFlickrBase58String(Int64.MaxValue).Consume();
        }

        [Benchmark]
        public static void FromFlickrBase58String_MaxValue()
        {
            Int64Encoder.FromFlickrBase58String("npL6MjP8Qfc").Consume();
        }
    }
}
