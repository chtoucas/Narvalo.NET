namespace Narvalo.Benchmark
{
    using System;
    using System.Globalization;
    using NodaTime;

    public struct BenchMetric : IEquatable<BenchMetric>
    {
        internal static readonly IBenchMetricFormatter DefaultFormatter
            = new BenchMetricFormatter();

        readonly Duration _duration;
        readonly int _iterations;
        readonly string _name;

        public BenchMetric(string name, Duration duration, int iterations)
        {
            Require.NotNullOrEmpty(name, "name");

            _name = name;
            _duration = duration;
            _iterations = iterations;
        }

        internal static BenchMetric Create(Benchmark benchmark, Duration duration)
        {
            Require.NotNull(benchmark, "benchmark");

            return new BenchMetric(
                benchmark.Name,
                duration,
                benchmark.Iterations
            );
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

        #region IEquatable<BenchMetric>

        public bool Equals(BenchMetric other)
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
            if (!(obj is BenchMetric)) {
                return false;
            }

            return Equals((BenchMetric)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            // FIXME
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

        #region Opérateurs.

        /// <summary />
        public static bool operator ==(BenchMetric left, BenchMetric right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(BenchMetric left, BenchMetric right)
        {
            return !left.Equals(right);
        }

        #endregion

        public string ToString(IBenchMetricFormatter fmt)
        {
            Require.NotNull(fmt, "fmt");

            return fmt.Format(CultureInfo.CurrentCulture, this);
        }
    }
}