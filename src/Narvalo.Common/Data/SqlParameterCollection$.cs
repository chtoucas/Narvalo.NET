﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

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
            Require.Object(@this);

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
            Acknowledge.Object(@this);

            @this.Add(parameterName, parameterType).AssumeNotNull().Value = value;
        }

        public static void AddParameterOrNull<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T? value)
            where T : struct
        {
            Require.Object(@this);

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
            Acknowledge.Object(@this);

            if (value.HasValue)
            {
                @this.Add(parameterName, parameterType).AssumeNotNull().Value = value.Value;
            }
            else
            {
                @this.Add(parameterName, parameterType).AssumeNotNull().Value = DBNull.Value;
            }
        }

        public static void AddParameterOrNull<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T value)
            where T : class
        {
            Require.Object(@this);

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
            Acknowledge.Object(@this);

            @this.AddParameterOrNullUnchecked(parameterName, parameterType, value, value != null);
        }

        public static void AddParameterOrNull<T>(
            this SqlParameterCollection @this,
            string parameterName,
            SqlDbType parameterType,
            T value,
            bool condition)
        {
            Require.Object(@this);

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
            Acknowledge.Object(@this);

            if (condition)
            {
                @this.Add(parameterName, parameterType).AssumeNotNull().Value = value;
            }
            else
            {
                @this.Add(parameterName, parameterType).AssumeNotNull().Value = DBNull.Value;
            }
        }
    }
}