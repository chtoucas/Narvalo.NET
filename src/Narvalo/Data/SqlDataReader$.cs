namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;
    using Narvalo.Diagnostics;
    using Narvalo.Fx;

    public static class SqlDataReaderExtensions
    {
        #region > Accès par position <

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlBinary(ordinal).ToMaybe();
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlBoolean(ordinal).ToMaybe();
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlByte(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlBytes(ordinal).ToMaybe();
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlChars(ordinal).ToMaybe();
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlDateTime(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlDecimal(ordinal).ToMaybe();
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlDouble(ordinal).ToMaybe();
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlGuid(ordinal).ToMaybe();
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlInt16(ordinal).ToMaybe();
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlInt32(ordinal).ToMaybe();
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlInt64(ordinal).ToMaybe();
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlMoney(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetString(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlString(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetXml(this SqlDataReader rdr, int ordinal)
        {
            Requires.NotNull(rdr);

            return rdr.GetSqlXml(ordinal).ToMaybe();
        }

        #endregion

        #region > Accès par nom <

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetBinary(rdr.GetOrdinal(name));
        }

        public static Maybe<bool> MayGetBoolean(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetBoolean(rdr.GetOrdinal(name));
        }

        public static Maybe<byte> MayGetByte(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetByte(rdr.GetOrdinal(name));
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetBytes(rdr.GetOrdinal(name));
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetChars(rdr.GetOrdinal(name));
        }

        public static Maybe<DateTime> MayGetDateTime(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetDateTime(rdr.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetDecimal(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetDecimal(rdr.GetOrdinal(name));
        }

        public static Maybe<double> MayGetDouble(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetDouble(rdr.GetOrdinal(name));
        }

        public static Maybe<Guid> MayGetGuid(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetGuid(rdr.GetOrdinal(name));
        }

        public static Maybe<short> MayGetInt16(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetInt16(rdr.GetOrdinal(name));
        }

        public static Maybe<int> MayGetInt32(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetInt32(rdr.GetOrdinal(name));
        }

        public static Maybe<long> MayGetInt64(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetInt64(rdr.GetOrdinal(name));
        }

        public static Maybe<decimal> MayGetMoney(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetMoney(rdr.GetOrdinal(name));
        }

        public static Maybe<string> MayGetString(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetString(rdr.GetOrdinal(name));
        }

        public static Maybe<string> MayGetXml(this SqlDataReader rdr, string name)
        {
            Requires.NotNull(rdr);

            return rdr.MayGetXml(rdr.GetOrdinal(name));
        }

        #endregion
    }
}
