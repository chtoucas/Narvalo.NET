// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

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

            var parameter = @this.Add(parameterName, parameterType);

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

            var parameter = @this.Add(parameterName, parameterType);

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

            var parameter = @this.Add(parameterName, parameterType);

            if (value != null)
            {
                parameter.Value = value;
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
            T value,
            bool condition)
        {
            Require.NotNull(@this, nameof(@this));

            var parameter = @this.Add(parameterName, parameterType);

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