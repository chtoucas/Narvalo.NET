// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    using Narvalo.Properties;

    internal static class Failure
    {
        public static ArgumentOutOfRangeException OutOfRange<T>(T value, T minInclusive, T maxInclusive, string parameterName)
            where T : IComparable<T>
            => new ArgumentOutOfRangeException(
                parameterName,
                value,
                Format.Current(
                    Strings_Futures.ArgumentOutOfRange,
                    parameterName,
                    minInclusive,
                    maxInclusive));

        public static ArgumentOutOfRangeException NotGreaterThan<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
            => new ArgumentOutOfRangeException(
                parameterName,
                value,
                Format.Current(
                    Strings_Futures.ArgumentOutOfRange_NotGreaterThan,
                    parameterName,
                    minValue));

        public static ArgumentOutOfRangeException NotGreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
            => new ArgumentOutOfRangeException(
                parameterName,
                value,
                Format.Current(
                    Strings_Futures.ArgumentOutOfRange_NotGreaterThanOrEqualTo,
                    parameterName,
                    minValue));

        public static ArgumentOutOfRangeException NotLessThan<T>(T value, T maxValue, string parameterName)
            where T : IComparable<T>
            => new ArgumentOutOfRangeException(
                parameterName,
                value,
                Format.Current(
                    Strings_Futures.ArgumentOutOfRange_NotLessThan,
                    parameterName,
                    maxValue));

        public static ArgumentOutOfRangeException NotLessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : IComparable<T>
            => new ArgumentOutOfRangeException(
                parameterName,
                value,
                Format.Current(
                    Strings_Futures.ArgumentOutOfRange_NotLessThanOrEqualTo,
                    parameterName,
                    maxValue));
    }
}
