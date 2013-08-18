namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;
    using Narvalo;
    using Narvalo.Fx;

    public static class SqlDataReaderExtensions
    {
        public static Maybe<byte[]> MayGetBinary(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlBinary(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetBinary(rdr.GetOrdinal(name));
        }


        public static bool GetBoolean(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetBoolean(rdr.GetOrdinal(name));
        }

        public static bool GetBoolean(this SqlDataReader rdr, int ordinal, bool defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlBoolean(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static bool GetBoolean(this SqlDataReader rdr, string name, bool defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetBoolean(rdr.GetOrdinal(name), defaultValue);
        }

        public static bool? GetNullableBoolean(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlBoolean(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static bool? GetNullableBoolean(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableBoolean(rdr.GetOrdinal(name));
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlBoolean(ordinal).ToMaybe();
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetBoolean(rdr.GetOrdinal(name));
        }


        public static byte GetByte(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetByte(rdr.GetOrdinal(name));
        }

        public static byte GetByte(this SqlDataReader rdr, int ordinal, byte defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlByte(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static byte GetByte(this SqlDataReader rdr, string name, byte defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetByte(rdr.GetOrdinal(name), defaultValue);
        }

        public static byte? GetNullableByte(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlByte(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static byte? GetNullableByte(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableByte(rdr.GetOrdinal(name));
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlByte(ordinal).ToMaybe();
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetByte(rdr.GetOrdinal(name));
        }


        public static Maybe<byte[]> MayGetBytes(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlBytes(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetBytes(rdr.GetOrdinal(name));
        }


        public static Maybe<char[]> MayGetChars(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlChars(ordinal).ToMaybe();
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetChars(rdr.GetOrdinal(name));
        }


        public static DateTime GetDateTime(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetDateTime(rdr.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader rdr, int ordinal, DateTime defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlDateTime(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static DateTime GetDateTime(this SqlDataReader rdr, string name, DateTime defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetDateTime(rdr.GetOrdinal(name), defaultValue);
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlDateTime(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableDateTime(rdr.GetOrdinal(name));
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlDateTime(ordinal).ToMaybe();
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetDateTime(rdr.GetOrdinal(name));
        }


        public static decimal GetDecimal(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetDecimal(rdr.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader rdr, int ordinal, decimal defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlDecimal(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetDecimal(this SqlDataReader rdr, string name, decimal defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetDecimal(rdr.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableDecimal(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlDecimal(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableDecimal(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableDecimal(rdr.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlDecimal(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetDecimal(rdr.GetOrdinal(name));
        }


        public static double GetDouble(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetDouble(rdr.GetOrdinal(name));
        }

        public static double GetDouble(this SqlDataReader rdr, int ordinal, double defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlDouble(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static double GetDouble(this SqlDataReader rdr, string name, double defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetDouble(rdr.GetOrdinal(name), defaultValue);
        }

        public static double? GetNullableDouble(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlDouble(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static double? GetNullableDouble(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableDouble(rdr.GetOrdinal(name));
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlDouble(ordinal).ToMaybe();
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetDouble(rdr.GetOrdinal(name));
        }


        public static Guid GetGuid(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetGuid(rdr.GetOrdinal(name));
        }

        public static Guid GetGuid(this SqlDataReader rdr, int ordinal, Guid defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlGuid(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static Guid GetGuid(this SqlDataReader rdr, string name, Guid defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetGuid(rdr.GetOrdinal(name), defaultValue);
        }

        public static Guid? GetNullableGuid(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlGuid(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static Guid? GetNullableGuid(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableGuid(rdr.GetOrdinal(name));
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlGuid(ordinal).ToMaybe();
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetGuid(rdr.GetOrdinal(name));
        }


        public static short GetInt16(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetInt16(rdr.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader rdr, int ordinal, short defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlInt16(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static short GetInt16(this SqlDataReader rdr, string name, short defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetInt16(rdr.GetOrdinal(name), defaultValue);
        }

        public static short? GetNullableInt16(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlInt16(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static short? GetNullableInt16(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableInt16(rdr.GetOrdinal(name));
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlInt16(ordinal).ToMaybe();
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetInt16(rdr.GetOrdinal(name));
        }


        public static int GetInt32(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetInt32(rdr.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader rdr, int ordinal, int defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlInt32(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static int GetInt32(this SqlDataReader rdr, string name, int defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetInt32(rdr.GetOrdinal(name), defaultValue);
        }

        public static int? GetNullableInt32(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

             var value = rdr.GetSqlInt32(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static int? GetNullableInt32(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableInt32(rdr.GetOrdinal(name));
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlInt32(ordinal).ToMaybe();
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetInt32(rdr.GetOrdinal(name));
        }


        public static long GetInt64(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetInt64(rdr.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader rdr, int ordinal, long defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlInt64(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static long GetInt64(this SqlDataReader rdr, string name, long defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetInt64(rdr.GetOrdinal(name), defaultValue);
        }

        public static long? GetNullableInt64(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlInt64(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static long? GetNullableInt64(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableInt64(rdr.GetOrdinal(name));
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlInt64(ordinal).ToMaybe();
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetInt64(rdr.GetOrdinal(name));
        }


        // NB: Pas de méthode GetMoney(this SqlDataReader rdr, string name)

        public static decimal GetMoney(this SqlDataReader rdr, int ordinal, decimal defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlMoney(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetMoney(this SqlDataReader rdr, string name, decimal defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetMoney(rdr.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableMoney(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlMoney(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableMoney(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetNullableMoney(rdr.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlMoney(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetMoney(rdr.GetOrdinal(name));
        }


        // > Valeurs de type référence <


        public static string GetString(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            
            return rdr.GetString(rdr.GetOrdinal(name));
        }

        public static string GetString(this SqlDataReader rdr, int ordinal, string defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlString(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetString(this SqlDataReader rdr, string name, string defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetString(rdr.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetString(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlString(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetString(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetString(rdr.GetOrdinal(name));
        }


        // NB: Pas de méthode GetXml(this SqlDataReader rdr, string name)

        public static string GetXml(this SqlDataReader rdr, int ordinal, string defaultValue)
        {
            Requires.Object(rdr);

            var value = rdr.GetSqlXml(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetXml(this SqlDataReader rdr, string name, string defaultValue)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");
            return rdr.GetXml(rdr.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetXml(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlXml(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetXml(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.MayGetXml(rdr.GetOrdinal(name));
        }
    }
}
