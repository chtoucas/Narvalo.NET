// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="IDataRecord"/>.
    /// </summary>
    [SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule",
        Justification = "[Intentionally] IDataRecord name is defined by the framework.")]
    public static partial class DataRecordExtensions
    {
        public static bool GetBoolean(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetBooleanUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static bool GetBooleanUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetBoolean(@this.GetOrdinal(name));
        }

        public static byte GetByte(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetByteUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static byte GetByteUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetByte(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetDateTimeUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static DateTime GetDateTimeUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetDateTime(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetDecimalUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static decimal GetDecimalUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetDecimal(@this.GetOrdinal(name));
        }

        public static double GetDouble(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetDoubleUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static double GetDoubleUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetDouble(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetGuidUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Guid GetGuidUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetGuid(@this.GetOrdinal(name));
        }

        public static short GetInt16(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetInt16Unsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static short GetInt16Unsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetInt16(@this.GetOrdinal(name));
        }

        public static int GetInt32(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetInt32Unsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static int GetInt32Unsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetInt32(@this.GetOrdinal(name));
        }

        public static long GetInt64(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetInt64Unsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static long GetInt64Unsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetInt64(@this.GetOrdinal(name));
        }
    }

    /// <content>
    /// Implements extensions for reference types.
    /// </content>
    public static partial class DataRecordExtensions
    {
        public static string GetString(this IDataRecord @this, string name)
        {
            Require.Object(@this);

            return @this.GetStringUnsafe(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static string GetStringUnsafe(this IDataRecord @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetString(@this.GetOrdinal(name));
        }
    }
}
