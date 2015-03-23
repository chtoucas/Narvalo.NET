// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public sealed partial class BenchmarkMetric : IFormattable
    {
        private const long TICKS_PER_SECOND = 10000000L;

        private readonly bool _fixedTime;
        private readonly int _iterations;
        private readonly TimeSpan _duration;
        private readonly string _categoryName;
        private readonly string _name;

        public BenchmarkMetric(string categoryName, string name, TimeSpan duration, int iterations)
            : this(categoryName, name, duration, iterations, true)
        {
            Contract.Requires(categoryName != null && categoryName.Length > 0);
            Contract.Requires(name != null && name.Length > 0);
            Contract.Requires(iterations > 0);
            Contract.Requires(duration.Ticks > 0L);
        }

        public BenchmarkMetric(string categoryName, string name, TimeSpan duration, int iterations, bool fixedTime)
        {
            Require.NotNullOrEmpty(categoryName, "categoryName");
            Require.NotNullOrEmpty(name, "name");
            Require.GreaterThan(iterations, 0, "iterations");
            Require.GreaterThan(duration.Ticks, 0L, "duration.Ticks");

            _categoryName = categoryName;
            _name = name;
            _duration = duration;
            _iterations = iterations;
            _fixedTime = fixedTime;
        }

        public long CallsPerSecond
        {
            get { return TICKS_PER_SECOND * Iterations / Duration.Ticks; }
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

        public TimeSpan Duration
        {
            get
            {
                Contract.Ensures(Contract.Result<TimeSpan>().Ticks > 0L);

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

#if CONTRACTS_FULL // [Ignore] Contract Class and Object Invariants.

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

    /// <content>
    /// Implements the <see cref="IFormattable"/> interface.
    /// </content>
    public partial class BenchmarkMetric
    {
        // https://msdn.microsoft.com/en-us/library/26etazsy.aspx
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return CallsPerSecond.ToString("#,0", CultureInfo.CurrentCulture)
                + " call/s; " + Name;
        }

        [SuppressMessage("Microsoft.Contracts", "Suggestion-18-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    var result = formatter.Format(format, this, formatProvider);

                    Contract.Assume(result != null, "ICustomFormatter.Format() returned a null string.");

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
                        Strings_Common.BenchmarkMetric_MetricFormat,
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
}
