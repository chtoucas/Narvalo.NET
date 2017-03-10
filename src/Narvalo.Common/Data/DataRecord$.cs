// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;

    /// <summary>
    /// Provides extension methods for <see cref="IDataRecord"/>.
    /// </summary>
    public static partial class DataRecordExtensions
    {
        public static bool GetBoolean(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetBoolean(@this.GetOrdinal(name));
        }

        public static byte GetByte(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetByte(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDateTime(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDecimal(@this.GetOrdinal(name));
        }

        public static double GetDouble(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDouble(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetGuid(@this.GetOrdinal(name));
        }

        public static short GetInt16(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt16(@this.GetOrdinal(name));
        }

        public static int GetInt32(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt32(@this.GetOrdinal(name));
        }

        public static long GetInt64(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt64(@this.GetOrdinal(name));
        }
    }

    // Extensions for reference types.
    public static partial class DataRecordExtensions
    {
        public static string GetString(this IDataRecord @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetString(@this.GetOrdinal(name));
        }
    }
}
