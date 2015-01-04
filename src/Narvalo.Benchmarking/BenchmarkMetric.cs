// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Globalization;
    using Narvalo;
    using NodaTime;

    public struct BenchmarkMetric : IEquatable<BenchmarkMetric>
    {
        internal static readonly IBenchmarkMetricFormatter DefaultFormatter
            = new BenchmarkMetricFormatter();

        readonly Duration _duration;
        readonly int _iterations;
        readonly string _name;

        public BenchmarkMetric(string name, Duration duration, int iterations)
        {
            Require.NotNullOrEmpty(name, "name");

            _name = name;
            _duration = duration;
            _iterations = iterations;
        }

        public long CallsPerSecond
        {
            get { return NodaConstants.TicksPerSecond * Iterations / Duration.Ticks; }
        }

        public Duration Duration { get { return _duration; } }

        public int Iterations { get { return _iterations; } }

        public string Name { get { return _name; } }

        public long TicksPerCall
        {
            get { return Duration.Ticks / Iterations; }
        }

        #region Opérateurs.

        /// <summary />
        public static bool operator ==(BenchmarkMetric left, BenchmarkMetric right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(BenchmarkMetric left, BenchmarkMetric right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region IEquatable<BenchMetric>

        public bool Equals(BenchmarkMetric other)
        {
            return Iterations == other.Iterations
                && Duration == other.Duration
                && Name == other.Name;
        }

        #endregion

        #region Surchages d'Object.

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is BenchmarkMetric)) {
                return false;
            }

            return Equals((BenchmarkMetric)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            // FIXME: GetHashCode
            return Iterations
                ^ Duration.GetHashCode()
                ^ Name.GetHashCode();
        }

        /// <summary />
        public override string ToString()
        {
            return ToString(DefaultFormatter);
        }

        #endregion

        public string ToString(IBenchmarkMetricFormatter formatter)
        {
            Require.NotNull(formatter, "fmt");

            return formatter.Format(CultureInfo.CurrentCulture, this);
        }

        internal static BenchmarkMetric Create(Benchmark benchmark, Duration duration)
        {
            Require.NotNull(benchmark, "benchmark");

            return new BenchmarkMetric(
                benchmark.Name,
                duration,
                benchmark.Iterations);
        }
    }
}
