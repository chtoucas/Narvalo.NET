// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Narvalo;
    using NodaTime;

    public partial struct BenchmarkMetric
        : IEquatable<BenchmarkMetric>, IFormattable
    {
        private readonly string _categoryName;
        private readonly Duration _duration;
        private readonly int _iterations;
        private readonly string _name;
        private readonly bool _fixedTime;

        public BenchmarkMetric(string categoryName, string name, Duration duration, int iterations)
            : this(categoryName, name, duration, iterations, true) { }

        public BenchmarkMetric(string categoryName, string name, Duration duration, int iterations, bool fixedTime)
        {
            Require.NotNullOrEmpty(categoryName, "categoryName");
            Require.NotNullOrEmpty(name, "name");
            Require.GreaterThanOrEqualTo(iterations, 1, "iterations");
            Require.GreaterThanOrEqualTo(duration, Duration.Epsilon, "duration");

            _categoryName = categoryName;
            _name = name;
            _duration = duration;
            _iterations = iterations;
            _fixedTime = fixedTime;
        }

        public long CallsPerSecond
        {
            get { return NodaConstants.TicksPerSecond * Iterations / Duration.Ticks; }
        }

        public string CategoryName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return _categoryName;
            }
        }

        public Duration Duration
        {
            get
            {
                Contract.Ensures(Contract.Result<Duration>() > Duration.Zero);

                return _duration;
            }
        }

        public int Iterations
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                return _iterations;
            }
        }

        public bool FixedTime { get { return _fixedTime; } }


        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return _name;
            }
        }

        public double TicksPerCall
        {
            get { return (double)Duration.Ticks / Iterations; }
        }
    }

    // Implements the IFormattable interface.
    // https://msdn.microsoft.com/en-us/library/26etazsy.aspx
    public partial struct BenchmarkMetric
    {
        /// <summary />
        public override string ToString()
        {
            return CallsPerSecond.ToString("#,0", CultureInfo.CurrentCulture)
                + " call/s; " + Name;
        }

        /// <summary />
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary />
        public string ToString(string format, IFormatProvider formatProvider)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, formatProvider);
                }
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            switch (format)
            {
                case "n":
                    return Name;
                case "d":
                    return String.Format(
                        formatProvider,
                        Strings_Benchmarking.MetricFormat,
                        Name,
                        CallsPerSecond,
                        Iterations,
                        Duration.Ticks,
                        TicksPerCall);
                case "G": // Same as ToString().
                default:
                    return ToString();
            }
        }
    }

    // Implements the IEquatable<BenchmarkMetric> interface.
    public partial struct BenchmarkMetric
    {
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

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is BenchmarkMetric))
            {
                return false;
            }

            return Equals((BenchmarkMetric)obj);
        }

        /// <summary />
        public bool Equals(BenchmarkMetric other)
        {
            return _iterations == other._iterations
                && _duration == other._duration
                && _fixedTime == other._fixedTime
                && _categoryName == other._categoryName
                && _name == other._name;
        }

        /// <summary />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + _iterations;
                hash = hash * 23 + _duration.GetHashCode();
                hash = hash * 23 + _categoryName.GetHashCode();
                hash = hash * 23 + _name.GetHashCode();
                hash = hash * 23 + _fixedTime.GetHashCode();

                return hash;
            }
        }
    }
}
