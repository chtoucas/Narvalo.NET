// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides extension methods for <see cref="IDataRecord"/>.
    /// </summary>
    public static partial class DataRecordExtensions
    {
        public static bool GetBoolean(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetBooleanUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static bool GetBooleanUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetBoolean(@this.GetOrdinal(name));
        }

        public static byte GetByte(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetByteUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static byte GetByteUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetByte(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDateTimeUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static DateTime GetDateTimeUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetDateTime(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDecimalUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static decimal GetDecimalUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetDecimal(@this.GetOrdinal(name));
        }

        public static double GetDouble(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDoubleUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static double GetDoubleUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetDouble(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetGuidUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Guid GetGuidUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetGuid(@this.GetOrdinal(name));
        }

        public static short GetInt16(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt16Unchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static short GetInt16Unchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetInt16(@this.GetOrdinal(name));
        }

        public static int GetInt32(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt32Unchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static int GetInt32Unchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetInt32(@this.GetOrdinal(name));
        }

        public static long GetInt64(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt64Unchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static long GetInt64Unchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetInt64(@this.GetOrdinal(name));
        }
    }

    // Extensions for reference types.
    public static partial class DataRecordExtensions
    {
        public static string GetString(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetStringUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static string GetStringUnchecked(this IDataRecord @this, string name)
        {
            Expect.NotNull(@this);

            return @this.GetString(@this.GetOrdinal(name));
        }
    }
}
