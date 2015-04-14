// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using Narvalo.BenchmarkCommon;
    using Narvalo.Internal;

    public static class MaybeBenchmarks
    {
        private static readonly Maybe<string> s_SampleSome = Maybe.Of("Some");
        private static readonly Maybe<string> s_SampleNone = Maybe<string>.None;

        [Benchmark]
        public static void None()
        {
            Maybe<string>.None.Consume();
        }

        [Benchmark]
        public static void Create_ReferenceType()
        {
            Maybe.Of("Some").Consume();
        }

        [Benchmark]
        public static void Create_ValueType()
        {
            Maybe.Of(1).Consume();
        }

        [Benchmark]
        public static void Create_NullReferenceType()
        {
            Maybe.Of<string>(null).Consume();
        }

        [Benchmark]
        public static void Create_NullValueType()
        {
            Maybe.Of<int>(null).Consume();
        }

        [Benchmark]
        public static void ValueOrDefault()
        {
            s_SampleSome.ValueOrDefault().Consume();
        }

        [Benchmark]
        public static void ValueOrElse()
        {
            s_SampleNone.ValueOrElse("Other").Consume();
        }
    }
}
