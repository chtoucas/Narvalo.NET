// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

// The "Require" class is largely taken from:
// http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    internal static class Require
    {
        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null) {
                throw new ArgumentNullException("this", "The object 'this' is null.");
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static T Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null) {
                throw new ArgumentNullException("value", "The property value is null.");
            }

            Contract.EndContractBlock();

            return value;
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static string PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0) {
                throw new ArgumentException("The property value is empty.", "value");
            }

            Contract.EndContractBlock();

            return value;
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull(parameterName);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0) {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "The parameter {0} is empty.",
                        parameterName),
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void InRange(int value, int minValue, int maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not in range {0} / {1}.",
                    minValue,
                    maxValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void InRange(long value, long minValue, long maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not in range {0} / {1}.",
                    minValue,
                    maxValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            if (value < minValue) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not greater than or equal to {0}.",
                    minValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            if (value < minValue) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not greater than or equal to {0}.",
                    minValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(minValue) < 0) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not greater than or equal to {0}.",
                    minValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not less than or equal to {0}.",
                    maxValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(long value, long maxValue, string parameterName)
        {
            if (value > maxValue) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not less than or equal to {0}.",
                    maxValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : IComparable<T>
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(maxValue) > 0) {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The value is not less than or equal to {0}.",
                    maxValue);
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        static class ExceptionFactory
        {
            public static ArgumentNullException ArgumentNull(string parameterName)
            {
                var message = String.Format(
                    CultureInfo.InvariantCulture,
                    "The parameter {0} is null.",
                    parameterName);
                return new ArgumentNullException(parameterName, message);
            }
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
        sealed class ValidatedNotNullAttribute : Attribute { }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Single file containing only internal classes and included in all MVP projects.")]
    internal static class DebugCheck
    {
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value)
        {
            Debug.Assert(value != null, "The value is null.");
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value)
        {
            NotNull(value);
            Debug.Assert(value.Length != 0, "The value is empty.");
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsEnum(Type type)
        {
            NotNull(type);
            Debug.Assert(type.IsEnum, type.FullName, "The type must be of enum type.");
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsValueType(Type type)
        {
            NotNull(type);
            Debug.Assert(type.IsValueType, type.FullName, "The type must be a value type.");
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Single file containing only internal classes and included in all MVP projects.")]
    internal static class __Tracer
    {
        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(object source, string format, params string[] messages)
        {
            Trace_(TraceLevel.Info, source, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(Type sourceType, string format, params string[] messages)
        {
            Trace_(TraceLevel.Info, sourceType, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(object source, string message)
        {
            Trace_(TraceLevel.Info, source, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(Type sourceType, string message)
        {
            Trace_(TraceLevel.Info, sourceType, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(object source, string format, params string[] messages)
        {
            Trace_(TraceLevel.Warning, source, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(Type sourceType, string format, params string[] messages)
        {
            Trace_(TraceLevel.Warning, sourceType, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(object source, string message)
        {
            Trace_(TraceLevel.Warning, source, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(Type sourceType, string message)
        {
            Trace_(TraceLevel.Warning, sourceType, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(object source, string format, params string[] messages)
        {
            Trace_(TraceLevel.Error, source, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(Type sourceType, string format, params string[] messages)
        {
            Trace_(TraceLevel.Error, sourceType, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(object source, string message)
        {
            Trace_(TraceLevel.Error, source, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(Type sourceType, string message)
        {
            Trace_(TraceLevel.Error, sourceType, message);
        }

        static void Trace_(TraceLevel level, object source, string format, params string[] messages)
        {
            Trace_(level, source.GetType(), format, messages);
        }

        static void Trace_(TraceLevel level, Type sourceType, string format, params string[] messages)
        {
            Trace_(level, sourceType, String.Format(CultureInfo.InvariantCulture, format, messages));
        }

        static void Trace_(TraceLevel level, object source, string message)
        {
            Trace_(level, source.GetType(), message);
        }

        [SuppressMessage("Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Private method only.")]
        static void Trace_(TraceLevel level, Type sourceType, string message)
        {
            var msg = String.Format(
                CultureInfo.InvariantCulture,
                "[{0}] {1}",
                sourceType.Name,
                message);

            switch (level) {
                case TraceLevel.Error:
                    Trace.TraceError(msg);
                    break;

                case TraceLevel.Warning:
                    Trace.TraceWarning(msg);
                    break;

                case TraceLevel.Info:
                case TraceLevel.Verbose:
                default:
                    Trace.TraceInformation(msg);
                    break;
            }
        }
    }
}
