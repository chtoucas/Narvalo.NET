// Au mieux on implémente les méthodes suivantes :
// - T GetValue(this SqlDataReader @this, string name)
// - T GetValue(this SqlDataReader @this, int ordinal, T defaultValue)
// - T GetValue(this SqlDataReader @this, string name, T defaultValue)
// - T? GetNullableValue(this SqlDataReader @this, int ordinal)
// - T? GetNullableValue(this SqlDataReader @this, string name)
// - Maybe<T> MayGetValue(this SqlDataReader @this, int ordinal)
// - Maybe<T> MayGetValue(this SqlDataReader @this, string name)
//
// Notez que les méthodes suivantes ne sont pas implémentées :
// - GetMoney(this SqlDataReader @this, string name)
// - GetXml(this SqlDataReader @this, string name)

namespace Narvalo.Data
{

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Data.SqlClient.SqlDataReader"/>.
    /// </summary>
    public static class SqlDataReaderExtraExtensions
    {
        //// Boolean

        //public static Maybe<bool> MayGetBoolean(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlBoolean(ordinal).ToMaybe();
        //}

        //public static Maybe<bool> MayGetBoolean(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetBoolean(@this.GetOrdinal(name));
        //}

        //// Byte

        //public static Maybe<byte> MayGetByte(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlByte(ordinal).ToMaybe();
        //}

        //public static Maybe<byte> MayGetByte(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetByte(@this.GetOrdinal(name));
        //}

        //// DateTime

        //public static Maybe<DateTime> MayGetDateTime(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlDateTime(ordinal).ToMaybe();
        //}

        //public static Maybe<DateTime> MayGetDateTime(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetDateTime(@this.GetOrdinal(name));
        //}

        //// Decimal

        //public static Maybe<decimal> MayGetDecimal(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlDecimal(ordinal).ToMaybe();
        //}

        //public static Maybe<decimal> MayGetDecimal(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetDecimal(@this.GetOrdinal(name));
        //}

        //// Double

        //public static Maybe<double> MayGetDouble(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlDouble(ordinal).ToMaybe();
        //}

        //public static Maybe<double> MayGetDouble(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetDouble(@this.GetOrdinal(name));
        //}

        //// Guid

        //public static Maybe<Guid> MayGetGuid(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlGuid(ordinal).ToMaybe();
        //}

        //public static Maybe<Guid> MayGetGuid(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetGuid(@this.GetOrdinal(name));
        //}

        //// Int16

        //public static Maybe<short> MayGetInt16(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlInt16(ordinal).ToMaybe();
        //}

        //public static Maybe<short> MayGetInt16(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetInt16(@this.GetOrdinal(name));
        //}

        //// Int32

        //public static Maybe<int> MayGetInt32(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlInt32(ordinal).ToMaybe();
        //}

        //public static Maybe<int> MayGetInt32(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetInt32(@this.GetOrdinal(name));
        //}

        //// Int64

        //public static Maybe<long> MayGetInt64(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlInt64(ordinal).ToMaybe();
        //}

        //public static Maybe<long> MayGetInt64(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetInt64(@this.GetOrdinal(name));
        //}

        //// Money

        //public static Maybe<decimal> MayGetMoney(this SqlDataReader @this, int ordinal)
        //{
        //    Require.Object(@this);

        //    return @this.GetSqlMoney(ordinal).ToMaybe();
        //}

        //public static Maybe<decimal> MayGetMoney(this SqlDataReader @this, string name)
        //{
        //    Require.Object(@this);
        //    Require.NotNullOrEmpty(name, "name");

        //    return @this.MayGetMoney(@this.GetOrdinal(name));
        //}
    }
}
