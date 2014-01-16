// Cette classe est inspirée de :
// http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Narvalo.Internal;

    public static class Requires
    {
        #region > Null <

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null) {
                throw new ArgumentNullException("this", SR.Requires_ObjectNull);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null) {
                throw new ArgumentNullException("value", SR.Requires_PropertyNull);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0) {
                throw new ArgumentException(SR.Requires_PropertyEmpty, "value");
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null) {
                throw Failure.ArgumentNull(parameterName, SR.Requires_ArgumentNullFormat, parameterName);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0) {
                throw Failure.Argument(parameterName, SR.Requires_ArgumentEmptyFormat, parameterName);
            }
            Contract.EndContractBlock();
        }

        #endregion

        #region > InRange <

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void InRange(int value, int minValue, int maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_NotInRangeFormat,
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
                    SR.Requires_NotInRangeFormat,
                    minValue,
                    maxValue);
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
                    SR.Requires_NotInRangeFormat,
                    range.LowerEnd,
                    range.UpperEnd);
            }
            Contract.EndContractBlock();
        }

        #endregion

        #region > GreaterThanOrEqualTo <

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            // FIXME: SR ne marche pas dans les tests ?
            if (value < minValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    //SR.Requires_NotGreaterThanOrEqualToFormat,
                    "The value is not greater than or equal to {0}.",
                    minValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            // FIXME: SR ne marche pas dans les tests ?
            if (value < minValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    //SR.Requires_NotGreaterThanOrEqualToFormat,
                    "The value is not greater than or equal to {0}.",
                    minValue);
            }
            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
        {
            // FIXME: SR ne marche pas dans les tests ?
            if (value.CompareTo(minValue) < 0) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    //SR.Requires_NotGreaterThanOrEqualToFormat,
                    "The value is not greater than or equal to {0}.",
                    minValue);
            }
            Contract.EndContractBlock();
        }

        #endregion

        #region > LessThanOrEqualTo <

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue) {
                throw Failure.ArgumentOutOfRange(
                    parameterName,
                    value,
                    SR.Requires_NotLessThanOrEqualToFormat,
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
                    SR.Requires_NotLessThanOrEqualToFormat,
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
                    SR.Requires_NotLessThanOrEqualToFormat,
                    maxValue);
            }
            Contract.EndContractBlock();
        }

        #endregion
    }
}
