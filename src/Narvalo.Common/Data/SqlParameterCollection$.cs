// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="SqlParameterCollection"/>.
    /// </summary>
    public static class SqlParameterCollectionExtensions
    {
        public static void AddParameter(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            object value)
        {
            Require.NotNull(@this, nameof(@this));

            @this.AddParameterUnchecked(parameterName, parameterType, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterUnchecked(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            object value)
        {
            Expect.NotNull(@this);

            var parameter = @this.Add(parameterName, parameterType);
            Contract.Assume(parameter != null);

            parameter.Value = value;
        }

        public static void AddParameterOrNull<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T? value)
            where T : struct
        {
            Require.NotNull(@this, nameof(@this));

            @this.AddParameterOrNullUnchecked(parameterName, parameterType, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterOrNullUnchecked<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T? value)
            where T : struct
        {
            Expect.NotNull(@this);

            var parameter = @this.Add(parameterName, parameterType);
            Contract.Assume(parameter != null);

            if (value.HasValue)
            {
                parameter.Value = value.Value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }

        public static void AddParameterOrNull<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T value)
            where T : class
        {
            Require.NotNull(@this, nameof(@this));

            @this.AddParameterOrNullUnchecked(parameterName, parameterType, value, value != null);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterOrNullUnchecked<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T value)
            where T : class
        {
            Expect.NotNull(@this);

            @this.AddParameterOrNullUnchecked(parameterName, parameterType, value, value != null);
        }

        public static void AddParameterOrNull<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T value,
            bool condition)
        {
            Require.NotNull(@this, nameof(@this));

            @this.AddParameterOrNullUnchecked(parameterName, parameterType, value, condition);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterOrNullUnchecked<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T value,
            bool condition)
        {
            Expect.NotNull(@this);

            var parameter = @this.Add(parameterName, parameterType);
            Contract.Assume(parameter != null);

            if (condition)
            {
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }
    }
}