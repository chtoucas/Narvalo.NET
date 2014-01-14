namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;
    using Narvalo;
    using Narvalo.Fx;

    public static class SqlDataReaderExtensions
    {
        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlBinary(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetBinary(@this.GetOrdinal(name));
        }


        public static bool GetBoolean(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetBoolean(@this.GetOrdinal(name));
        }

        public static bool GetBoolean(this SqlDataReader @this, int ordinal, bool defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlBoolean(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static bool GetBoolean(this SqlDataReader @this, string name, bool defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetBoolean(@this.GetOrdinal(name), defaultValue);
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlBoolean(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableBoolean(@this.GetOrdinal(name));
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlBoolean(ordinal).ToMaybe();
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetBoolean(@this.GetOrdinal(name));
        }


        public static byte GetByte(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetByte(@this.GetOrdinal(name));
        }

        public static byte GetByte(this SqlDataReader @this, int ordinal, byte defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlByte(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static byte GetByte(this SqlDataReader @this, string name, byte defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetByte(@this.GetOrdinal(name), defaultValue);
        }

        public static byte? GetNullableByte(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlByte(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static byte? GetNullableByte(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableByte(@this.GetOrdinal(name));
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlByte(ordinal).ToMaybe();
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetByte(@this.GetOrdinal(name));
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlBytes(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetBytes(@this.GetOrdinal(name));
        }


        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlChars(ordinal).ToMaybe();
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetChars(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetDateTime(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader @this, int ordinal, DateTime defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlDateTime(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static DateTime GetDateTime(this SqlDataReader @this, string name, DateTime defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetDateTime(@this.GetOrdinal(name), defaultValue);
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlDateTime(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableDateTime(@this.GetOrdinal(name));
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlDateTime(ordinal).ToMaybe();
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetDateTime(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetDecimal(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlDecimal(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetDecimal(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetDecimal(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlDecimal(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableDecimal(@this.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlDecimal(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetDecimal(@this.GetOrdinal(name));
        }


        public static double GetDouble(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetDouble(@this.GetOrdinal(name));
        }

        public static double GetDouble(this SqlDataReader @this, int ordinal, double defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlDouble(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static double GetDouble(this SqlDataReader @this, string name, double defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetDouble(@this.GetOrdinal(name), defaultValue);
        }

        public static double? GetNullableDouble(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlDouble(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static double? GetNullableDouble(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableDouble(@this.GetOrdinal(name));
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlDouble(ordinal).ToMaybe();
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetDouble(@this.GetOrdinal(name));
        }


        public static Guid GetGuid(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetGuid(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this SqlDataReader @this, int ordinal, Guid defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlGuid(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static Guid GetGuid(this SqlDataReader @this, string name, Guid defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetGuid(@this.GetOrdinal(name), defaultValue);
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlGuid(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableGuid(@this.GetOrdinal(name));
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlGuid(ordinal).ToMaybe();
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetGuid(@this.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetInt16(@this.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader @this, int ordinal, short defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlInt16(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static short GetInt16(this SqlDataReader @this, string name, short defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetInt16(@this.GetOrdinal(name), defaultValue);
        }

        public static short? GetNullableInt16(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlInt16(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static short? GetNullableInt16(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableInt16(@this.GetOrdinal(name));
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlInt16(ordinal).ToMaybe();
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetInt16(@this.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetInt32(@this.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader @this, int ordinal, int defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlInt32(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static int GetInt32(this SqlDataReader @this, string name, int defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetInt32(@this.GetOrdinal(name), defaultValue);
        }

        public static int? GetNullableInt32(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

             var value = @this.GetSqlInt32(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static int? GetNullableInt32(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableInt32(@this.GetOrdinal(name));
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlInt32(ordinal).ToMaybe();
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetInt32(@this.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetInt64(@this.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader @this, int ordinal, long defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlInt64(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static long GetInt64(this SqlDataReader @this, string name, long defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");
            return @this.GetInt64(@this.GetOrdinal(name), defaultValue);
        }

        public static long? GetNullableInt64(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlInt64(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static long? GetNullableInt64(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");
            return @this.GetNullableInt64(@this.GetOrdinal(name));
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlInt64(ordinal).ToMaybe();
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetInt64(@this.GetOrdinal(name));
        }


        // NB: Pas de méthode GetMoney(this SqlDataReader @this, string name)

        public static decimal GetMoney(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlMoney(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetMoney(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetMoney(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            var value = @this.GetSqlMoney(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetNullableMoney(@this.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlMoney(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetMoney(@this.GetOrdinal(name));
        }

        // > Valeurs de type référence <

        public static string GetString(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");
            
            return @this.GetString(@this.GetOrdinal(name));
        }

        public static string GetString(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlString(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetString(this SqlDataReader @this, string name, string defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetString(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlString(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetString(@this.GetOrdinal(name));
        }

        // NB: Pas de méthode GetXml(this SqlDataReader @this, string name)

        public static string GetXml(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Requires.Object(@this);

            var value = @this.GetSqlXml(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetXml(this SqlDataReader @this, string name, string defaultValue)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.GetXml(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetXml(this SqlDataReader @this, int ordinal)
        {
            Requires.Object(@this);

            return @this.GetSqlXml(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetXml(this SqlDataReader @this, string name)
        {
            Requires.Object(@this);
            Requires.NotNullOrEmpty(name, "name");

            return @this.MayGetXml(@this.GetOrdinal(name));
        }
    }
}
