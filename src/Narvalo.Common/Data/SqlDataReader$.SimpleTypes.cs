namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;

    public static partial class SqlDataReaderExtensions
    {
        //// Boolean

        public static bool GetBoolean(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetBoolean(@this.GetOrdinal(name));
        }

        public static bool GetBoolean(this SqlDataReader @this, int ordinal, bool defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlBoolean(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static bool GetBoolean(this SqlDataReader @this, string name, bool defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetBoolean(@this.GetOrdinal(name), defaultValue);
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlBoolean(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableBoolean(@this.GetOrdinal(name));
        }

        //// Byte

        public static byte GetByte(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetByte(@this.GetOrdinal(name));
        }

        public static byte GetByte(this SqlDataReader @this, int ordinal, byte defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlByte(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static byte GetByte(this SqlDataReader @this, string name, byte defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetByte(@this.GetOrdinal(name), defaultValue);
        }

        public static byte? GetNullableByte(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlByte(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static byte? GetNullableByte(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableByte(@this.GetOrdinal(name));
        }

        //// DateTime

        public static DateTime GetDateTime(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetDateTime(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader @this, int ordinal, DateTime defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlDateTime(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static DateTime GetDateTime(this SqlDataReader @this, string name, DateTime defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetDateTime(@this.GetOrdinal(name), defaultValue);
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlDateTime(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableDateTime(@this.GetOrdinal(name));
        }

        //// Decimal

        public static decimal GetDecimal(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetDecimal(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlDecimal(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetDecimal(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetDecimal(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlDecimal(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableDecimal(@this.GetOrdinal(name));
        }

        //// Double

        public static double GetDouble(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetDouble(@this.GetOrdinal(name));
        }

        public static double GetDouble(this SqlDataReader @this, int ordinal, double defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlDouble(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static double GetDouble(this SqlDataReader @this, string name, double defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetDouble(@this.GetOrdinal(name), defaultValue);
        }

        public static double? GetNullableDouble(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlDouble(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static double? GetNullableDouble(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableDouble(@this.GetOrdinal(name));
        }

        //// Guid

        public static Guid GetGuid(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetGuid(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this SqlDataReader @this, int ordinal, Guid defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlGuid(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static Guid GetGuid(this SqlDataReader @this, string name, Guid defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetGuid(@this.GetOrdinal(name), defaultValue);
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlGuid(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableGuid(@this.GetOrdinal(name));
        }

        //// Int16

        public static short GetInt16(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetInt16(@this.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader @this, int ordinal, short defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlInt16(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static short GetInt16(this SqlDataReader @this, string name, short defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetInt16(@this.GetOrdinal(name), defaultValue);
        }

        public static short? GetNullableInt16(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlInt16(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static short? GetNullableInt16(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableInt16(@this.GetOrdinal(name));
        }

        //// Int32

        public static int GetInt32(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetInt32(@this.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader @this, int ordinal, int defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlInt32(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static int GetInt32(this SqlDataReader @this, string name, int defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetInt32(@this.GetOrdinal(name), defaultValue);
        }

        public static int? GetNullableInt32(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlInt32(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static int? GetNullableInt32(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetNullableInt32(@this.GetOrdinal(name));
        }

        //// Int64

        public static long GetInt64(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");

            return @this.GetInt64(@this.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader @this, int ordinal, long defaultValue)
        {
            Require.Object(@this);

            var value = @this.GetSqlInt64(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static long GetInt64(this SqlDataReader @this, string name, long defaultValue)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");
            return @this.GetInt64(@this.GetOrdinal(name), defaultValue);
        }

        public static long? GetNullableInt64(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            var value = @this.GetSqlInt64(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static long? GetNullableInt64(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(name, "name");
            return @this.GetNullableInt64(@this.GetOrdinal(name));
        }
    }
}
