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
            : this(categoryName, name, duration, iterations, true)
        {
            Contract.Requires(categoryName != null && categoryName.Length > 0);
            Contract.Requires(name != null && name.Length > 0);
            Contract.Requires(iterations > 0);
            Contract.Requires(duration.Ticks > 0L);
        }

        public BenchmarkMetric(string categoryName, string name, Duration duration, int iterations, bool fixedTime)
        {
            Require.NotNullOrEmpty(categoryName, "categoryName");
            Require.NotNullOrEmpty(name, "name");
            Require.GreaterThanOrEqualTo(iterations, 1, "iterations");
            Require.Condition(duration.Ticks > 0L, "duration");

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
                Contract.Ensures(Contract.Result<string>().Length != 0);

                return _categoryName;
            }
        }

        public Duration Duration
        {
            get
            {
                Contract.Ensures(Contract.Result<Duration>().Ticks > 0L);

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
                Contract.Ensures(Contract.Result<string>().Length != 0);

                return _name;
            }
        }

        public double TicksPerCall
        {
            get { return (double)Duration.Ticks / Iterations; }
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_categoryName != null && _categoryName.Length != 0);
            Contract.Invariant(_name != null && _name.Length != 0);
            Contract.Invariant(_iterations > 0);
            Contract.Invariant(_duration.Ticks > 0L);
        }

#endif
    }

    // Implements the IFormattable interface.
    // https://msdn.microsoft.com/en-us/library/26etazsy.aspx
    public partial struct BenchmarkMetric
    {
        /// <summary />
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return CallsPerSecond.ToString("#,0", CultureInfo.CurrentCulture)
                + " call/s; " + Name;
        }

        /// <summary />
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

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
                    var result = formatter.Format(format, this, formatProvider);

                    Contract.Assume(result != null, "ICustomFormatter.Format() should not have returned a null string.");

                    return result;
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
                default:  // NB: This includes the "null" case.
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

                hash = (23 * hash) + _iterations;
                hash = (23 * hash) + _duration.GetHashCode();
                hash = (23 * hash) + _categoryName.GetHashCode();
                hash = (23 * hash) + _name.GetHashCode();
                hash = (23 * hash) + _fixedTime.GetHashCode();

                return hash;
            }
        }
    }
}
