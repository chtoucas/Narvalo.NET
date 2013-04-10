namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Narvalo.Internal;
    using Narvalo.Resources;

    // Cf. http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx
    public static class Requires
    {
        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T value) where T : class
        {
            NotNull(value, "this");
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null) {
                throw Failure.ArgumentNull(parameterName);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0) {
                throw Failure.ArgumentEmpty(parameterName);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void InRange<T>(T value, Range<T> range, string parameterName)
            where T : IComparable<T>, IEquatable<T>
        {
            if (!range.Includes(value)) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotInRange,
                    range.LowerEnd,
                    range.UpperEnd);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void InRange(int value, int minValue, int maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotInRange,
                    minValue,
                    maxValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void InRange(long value, long minValue, long maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotInRange,
                    minValue,
                    maxValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            if (value < minValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotGreaterThanOrEqualTo,
                    minValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            if (value < minValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotGreaterThanOrEqualTo,
                    minValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
        {
            if (value.CompareTo(minValue) < 0) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotGreaterThanOrEqualTo,
                    minValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotLessThanOrEqualTo,
                    maxValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(long value, long maxValue, string parameterName)
        {
            if (value > maxValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotLessThanOrEqualTo,
                    maxValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : IComparable<T>
        {
            if (value.CompareTo(maxValue) > 0) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_IsNotLessThanOrEqualTo,
                    maxValue);
            }
            Contract.EndContractBlock();
        }
    }
}
