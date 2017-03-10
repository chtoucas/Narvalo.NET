// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;

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
            Require.NotNull(@this, nameof(@this));

            @this.Parameters.AddParameter(parameterName, parameterType, value);
        }

        public static void AddParameterOrNull<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T? value)
            where T : struct
        {
            Require.NotNull(@this, nameof(@this));

            @this.Parameters.AddParameterOrNull(parameterName, parameterType, value);
        }

        public static void AddParameterOrNull<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T value)
        {
            Require.NotNull(@this, nameof(@this));

            @this.Parameters.AddParameterOrNull(parameterName, parameterType, value, value != null);
        }

        public static void AddParameterOrNull<T>(
            this SqlCommand @this,
            string parameterName,
            SqlDbType parameterType,
            T value,
            bool condition)
        {
            Require.NotNull(@this, nameof(@this));

            @this.Parameters.AddParameterOrNull(parameterName, parameterType, value, condition);
        }
    }
}