namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;
    using Narvalo;
    using Narvalo.Fx;

    public static class SqlDataReaderExtensions
    {
        #region > Accès simple par nom <

        //public static bool GetBooleanByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    var value = rdr.GetSqlBoolean(ordinal);
        //    return value.IsNull ? false : value.Value;
        //}

        public static bool GetBoolean(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            var value = rdr.GetSqlBoolean(rdr.GetOrdinal(name));
            return value.IsNull ? false : value.Value;
        }

        //public static DateTime? GetDateTimeByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetDateTime(ordinal);
        //    }
        //}

        public static DateTime GetDateTime(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            return rdr.GetDateTime(rdr.GetOrdinal(name));
        }

        //public static decimal GetDecimalByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    var value = rdr.GetSqlDecimal(ordinal);
        //    return value.IsNull ? default(decimal) : value.Value;
        //}

        public static decimal GetDecimal(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            var value = rdr.GetSqlDecimal(rdr.GetOrdinal(name));
            return value.IsNull ? default(decimal) : value.Value;
        }

        //public static double GetDoubleByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    var value = rdr.GetSqlDouble(ordinal);
        //    return value.IsNull ? default(double) : value.Value;
        //}

        public static double GetDouble(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            var value = rdr.GetSqlDouble(rdr.GetOrdinal(name));

            return value.IsNull ? default(double) : value.Value;
        }

        //public static int GetInt16ByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    var value = rdr.GetSqlInt16(ordinal);
        //    return value.IsNull ? default(int) : value.Value;
        //}

        public static int GetInt16(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            var value = rdr.GetSqlInt16(rdr.GetOrdinal(name));

            return value.IsNull ? default(int) : value.Value;
        }

        //public static int GetInt32ByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    var value = rdr.GetSqlInt32(ordinal);

        //    return value.IsNull ? default(int) : value.Value;
        //}

        public static int GetInt32(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            var value = rdr.GetSqlInt32(rdr.GetOrdinal(name));

            return value.IsNull ? default(int) : value.Value;
        }

        //public static string GetStringByName(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    var value = rdr.GetSqlString(ordinal);

        //    return value.IsNull ? String.Empty : value.Value;
        //}

        public static string GetString(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);
            Requires.NotNullOrEmpty(name, "name");

            var value = rdr.GetSqlString(rdr.GetOrdinal(name));

            return value.IsNull ? String.Empty : value.Value;
        }

        //public static bool? GetNullableBooleanColumn(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlBoolean(ordinal).Value;
        //    }
        //}

        //public static bool? GetNullableBooleanColumn(this SqlDataReader rdr, string name)
        //{
        //    Requires.Object(rdr);
        //    Requires.NotNullOrEmpty(name, "name");

        //    int ordinal = rdr.GetOrdinal(name);
        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlBoolean(ordinal).Value;
        //    }
        //}

        //public static DateTime? GetDateTimeColumn(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetDateTime(ordinal);
        //    }
        //}

        //public static DateTime? GetDateTimeColumn(this SqlDataReader rdr, string name)
        //{
        //    Requires.Object(rdr);
        //    Requires.NotNullOrEmpty(name, "name");

        //    int ordinal = rdr.GetOrdinal(name);
        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetDateTime(ordinal);
        //    }
        //}

        //public static decimal? GetNullableDecimalColumn(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlDecimal(ordinal).Value;
        //    }
        //}

        //public static decimal? GetNullableDecimalColumn(this SqlDataReader rdr, string name)
        //{
        //    Requires.Object(rdr);

        //    int ordinal = rdr.GetOrdinal(name);
        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlDecimal(ordinal).Value;
        //    }
        //}

        //public static int? GetNullableInt16Column(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlInt16(ordinal).Value;
        //    }
        //}

        //public static int? GetNullableInt16Column(this SqlDataReader rdr, string name)
        //{
        //    Requires.Object(rdr);
        //    Requires.NotNullOrEmpty(name, "name");

        //    int ordinal = rdr.GetOrdinal(name);
        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlInt16(ordinal).Value;
        //    }
        //}

        //public static int? GetNullableInt32Column(this SqlDataReader rdr, int ordinal)
        //{
        //    Requires.Object(rdr);

        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlInt32(ordinal).Value;
        //    }
        //}

        //public static int? GetNullableInt32Column(this SqlDataReader rdr, string name)
        //{
        //    Requires.Object(rdr);
        //    Requires.NotNullOrEmpty(name, "name");

        //    int ordinal = rdr.GetOrdinal(name);
        //    if (rdr.IsDBNull(ordinal)) {
        //        return null;
        //    }
        //    else {
        //        return rdr.GetSqlInt32(ordinal).Value;
        //    }
        //}

        #endregion

        #region > Accès par position <

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlBinary(ordinal).ToMaybe();
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlBoolean(ordinal).ToMaybe();
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlByte(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlBytes(ordinal).ToMaybe();
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlChars(ordinal).ToMaybe();
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlDateTime(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlDecimal(ordinal).ToMaybe();
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlDouble(ordinal).ToMaybe();
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlGuid(ordinal).ToMaybe();
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlInt16(ordinal).ToMaybe();
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlInt32(ordinal).ToMaybe();
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlInt64(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlMoney(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetString(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlString(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetXml(this SqlDataReader rdr, int ordinal)
        {
            Requires.Object(rdr);

            return rdr.GetSqlXml(ordinal).ToMaybe();
        }

        #endregion

        #region > Accès par nom <

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetBinary(rdr.GetOrdinal(name));
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetBoolean(rdr.GetOrdinal(name));
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetByte(rdr.GetOrdinal(name));
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetBytes(rdr.GetOrdinal(name));
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetChars(rdr.GetOrdinal(name));
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetDateTime(rdr.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetDecimal(rdr.GetOrdinal(name));
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetDouble(rdr.GetOrdinal(name));
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetGuid(rdr.GetOrdinal(name));
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetInt16(rdr.GetOrdinal(name));
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetInt32(rdr.GetOrdinal(name));
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetInt64(rdr.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetMoney(rdr.GetOrdinal(name));
        }

        public static Maybe<string> MayGetString(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetString(rdr.GetOrdinal(name));
        }

        public static Maybe<string> MayGetXml(this SqlDataReader rdr, string name)
        {
            Requires.Object(rdr);

            return rdr.MayGetXml(rdr.GetOrdinal(name));
        }

        #endregion
    }
}
