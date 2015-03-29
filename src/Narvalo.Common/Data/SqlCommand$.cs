// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="SqlCommand"/>.
    /// </summary>
    public static class SqlCommandExtensions
    {
        public static void AddParameter(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            object value)
        {
            Require.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterUnsafe(parameterName, parameterType, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterUnsafe(
            this SqlCommand @this, 
            string parameterName,
            SqlDbType parameterType,
            object value)
        {
            Acknowledge.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterUnsafe(parameterName, parameterType, value);
        }

        public static void AddParameterOrNull<T>(
            this SqlCommand @this, 
            string parameterName,
            SqlDbType parameterType,
            T? value)
            where T : struct
        {
            Require.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterOrNullUnsafe(parameterName, parameterType, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterOrNullUnsafe<T>(
            this SqlCommand @this, 
            string parameterName,
            SqlDbType parameterType,
            T? value)
            where T : struct
        {
            Acknowledge.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterOrNullUnsafe(parameterName, parameterType, value);
        }

        public static void AddParameterOrNull<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T value)
        {
            Require.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterOrNullUnsafe(parameterName, parameterType, value, value != null);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterOrNullUnsafe<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T value)
        {
            Acknowledge.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterOrNullUnsafe(parameterName, parameterType, value, value != null);
        }

        public static void AddParameterOrNull<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T value,
            bool condition)
        {
            Require.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterOrNullUnsafe(parameterName, parameterType, value, condition);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static void AddParameterOrNullUnsafe<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T value,
            bool condition)
        {
            Acknowledge.Object(@this);

            @this.Parameters.AssumeNotNull().AddParameterOrNullUnsafe(parameterName, parameterType, value, condition);
        }
    }
}