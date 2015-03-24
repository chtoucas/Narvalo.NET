// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx;
    using Narvalo.Internal;

    /*!
     * Méthodes d'extension pour `SqlDataReader` 
     * =========================================
     * 
     * Objets de type valeur
     * ---------------------
     * 
     * On implémente les méthodes suivantes :
     * ```
     * T GetValue(this SqlDataReader @this, string name)
     * T GetValue(this SqlDataReader @this, int ordinal, T defaultValue)
     * T GetValue(this SqlDataReader @this, string name, T defaultValue)
     * T? GetNullableValue(this SqlDataReader @this, int ordinal)
     * T? GetNullableValue(this SqlDataReader @this, string name)
     * ```
     *     
     * Objets de type référence
     * ------------------------
     * 
     * On implémente les méthodes suivantes :
     * ```
     * Maybe<T> MayGetReference(this SqlDataReader @this, int ordinal)
     * Maybe<T> MayGetReference(this SqlDataReader @this, string name)
     * ```
     * et si cela a du sens :
     * ```
     * T GetReference(this SqlDataReader @this, string name)
     * T GetReference(this SqlDataReader @this, int ordinal, T defaultValue)
     * T GetReference(this SqlDataReader @this, string name, T defaultValue)
     * ```
     */

    /// <summary>
    /// Provides extension methods for <see cref="System.Data.SqlClient.SqlDataReader"/>.
    /// </summary>
    public static partial class SqlDataReaderExtensions
    {
        public static bool GetBoolean(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetBooleanUnsafe(name);
        }

        public static bool GetBooleanUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetBoolean(@this.GetOrdinal(name));
        }

        public static bool GetBoolean(this SqlDataReader @this, int ordinal, bool defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlBoolean(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static bool GetBoolean(this SqlDataReader @this, string name, bool defaultValue)
        {
            Require.Object(@this);

            return @this.GetBoolean(@this.GetOrdinal(name), defaultValue);
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlBoolean(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableBoolean(@this.GetOrdinal(name));
        }

        public static byte GetByte(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetByte(@this.GetOrdinal(name));
        }

        public static byte GetByte(this SqlDataReader @this, int ordinal, byte defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlByte(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static byte GetByte(this SqlDataReader @this, string name, byte defaultValue)
        {
            Require.Object(@this);

            return @this.GetByte(@this.GetOrdinal(name), defaultValue);
        }

        public static byte? GetNullableByte(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlByte(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static byte? GetNullableByte(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableByte(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetDateTimeUnsafe(name);
        }

        public static DateTime GetDateTimeUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetDateTime(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader @this, int ordinal, DateTime defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlDateTime(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static DateTime GetDateTime(this SqlDataReader @this, string name, DateTime defaultValue)
        {
            Require.Object(@this);

            return @this.GetDateTime(@this.GetOrdinal(name), defaultValue);
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlDateTime(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableDateTime(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetDecimal(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlDecimal(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetDecimal(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Require.Object(@this);

            return @this.GetDecimal(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlDecimal(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableDecimal(@this.GetOrdinal(name));
        }

        public static double GetDouble(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetDouble(@this.GetOrdinal(name));
        }

        public static double GetDouble(this SqlDataReader @this, int ordinal, double defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlDouble(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static double GetDouble(this SqlDataReader @this, string name, double defaultValue)
        {
            Require.Object(@this);

            return @this.GetDouble(@this.GetOrdinal(name), defaultValue);
        }

        public static double? GetNullableDouble(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlDouble(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static double? GetNullableDouble(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableDouble(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetGuid(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this SqlDataReader @this, int ordinal, Guid defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlGuid(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static Guid GetGuid(this SqlDataReader @this, string name, Guid defaultValue)
        {
            Require.Object(@this);

            return @this.GetGuid(@this.GetOrdinal(name), defaultValue);
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlGuid(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableGuid(@this.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetInt16(@this.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader @this, int ordinal, short defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlInt16(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static short GetInt16(this SqlDataReader @this, string name, short defaultValue)
        {
            Require.Object(@this);

            return @this.GetInt16(@this.GetOrdinal(name), defaultValue);
        }

        public static short? GetNullableInt16(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlInt16(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static short? GetNullableInt16(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableInt16(@this.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetInt32Unsafe(name);
        }

        public static int GetInt32Unsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetInt32(@this.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader @this, int ordinal, int defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlInt32(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static int GetInt32(this SqlDataReader @this, string name, int defaultValue)
        {
            Require.Object(@this);

            return @this.GetInt32(@this.GetOrdinal(name), defaultValue);
        }

        public static int? GetNullableInt32(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlInt32(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static int? GetNullableInt32(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableInt32(@this.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetInt64(@this.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader @this, int ordinal, long defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlInt64(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static long GetInt64(this SqlDataReader @this, string name, long defaultValue)
        {
            Require.Object(@this);

            return @this.GetInt64(@this.GetOrdinal(name), defaultValue);
        }

        public static long? GetNullableInt64(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlInt64(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static long? GetNullableInt64(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableInt64(@this.GetOrdinal(name));
        }
    }

    /// <content>
    /// Implements extensions for reference types.
    /// </content>
    public static partial class SqlDataReaderExtensions
    {
        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            return @this.MayGetBytesUnsafe(ordinal);
        }

        public static Maybe<byte[]> MayGetBytesUnsafe(this SqlDataReader @this, int ordinal)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.GetSqlBytes(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.MayGetBytesUnsafe(name);
        }

        public static Maybe<byte[]> MayGetBytesUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.MayGetBytes(@this.GetOrdinal(name));
        }

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            return @this.MayGetBinaryUnsafe(ordinal);
        }

        public static Maybe<byte[]> MayGetBinaryUnsafe(this SqlDataReader @this, int ordinal)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.GetSqlBinary(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.MayGetBinaryUnsafe(name);
        }

        public static Maybe<byte[]> MayGetBinaryUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.MayGetBinary(@this.GetOrdinal(name));
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            return @this.MayGetCharsUnsafe(ordinal);
        }

        [SuppressMessage("Gendarme.Rules.Portability", "MonoCompatibilityReviewRule",
            Justification = "[Intentionally] Missing method from Mono with no adequate replacement.")]
        public static Maybe<char[]> MayGetCharsUnsafe(this SqlDataReader @this, int ordinal)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<char[]>>() != null);

            return @this.GetSqlChars(ordinal).ToMaybe();
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.MayGetCharsUnsafe(name);
        }

        public static Maybe<char[]> MayGetCharsUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<char[]>>() != null);

            return @this.MayGetChars(@this.GetOrdinal(name));
        }

        public static string GetString(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetStringUnsafe(name);
        }

        public static string GetStringUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetString(@this.GetOrdinal(name));
        }

        public static string GetString(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Require.Object(@this);

            return @this.GetStringUnsafe(ordinal, defaultValue);
        }

        public static string GetStringUnsafe(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlString(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetString(this SqlDataReader @this, string name, string defaultValue)
        {
            Require.Object(@this);

            return @this.GetStringUnsafe(name, defaultValue);
        }

        public static string GetStringUnsafe(this SqlDataReader @this, string name, string defaultValue)
        {
            Contract.Requires(@this != null);

            return @this.GetString(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            return @this.MayGetStringUnsafe(ordinal);
        }

        public static Maybe<string> MayGetStringUnsafe(this SqlDataReader @this, int ordinal)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.GetSqlString(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.MayGetStringUnsafe(name);
        }

        public static Maybe<string> MayGetStringUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.MayGetString(@this.GetOrdinal(name));
        }

        public static decimal GetMoney(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Require.Object(@this);

            return @this.GetMoneyUnsafe(ordinal, defaultValue);
        }

        public static decimal GetMoneyUnsafe(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlMoney(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetMoney(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Require.Object(@this);

            return @this.GetMoneyUnsafe(name, defaultValue);
        }

        public static decimal GetMoneyUnsafe(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Contract.Requires(@this != null);

            return @this.GetMoney(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            return @this.GetNullableMoneyUnsafe(ordinal);
        }

        public static decimal? GetNullableMoneyUnsafe(this SqlDataReader @this, int ordinal)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlMoney(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableMoneyUnsafe(name);
        }

        public static decimal? GetNullableMoneyUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);

            return @this.GetNullableMoney(@this.GetOrdinal(name));
        }

        public static string GetXml(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Require.Object(@this);

            return @this.GetXmlUnsafe(ordinal, defaultValue);
        }

        public static string GetXmlUnsafe(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlXml(ordinal).AssumeNotNull();
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetXml(this SqlDataReader @this, string name, string defaultValue)
        {
            Require.Object(@this);

            return @this.GetXmlUnsafe(name, defaultValue);
        }

        public static string GetXmlUnsafe(this SqlDataReader @this, string name, string defaultValue)
        {
            Contract.Requires(@this != null);

            return @this.GetXml(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetXml(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);

            return @this.MayGetXmlUnsafe(ordinal);
        }

        public static Maybe<string> MayGetXmlUnsafe(this SqlDataReader @this, int ordinal)
        {
            Contract.Requires(@this != null);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.GetSqlXml(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetXml(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.MayGetXmlUnsafe(name);
        }

        public static Maybe<string> MayGetXmlUnsafe(this SqlDataReader @this, string name)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.MayGetXml(@this.GetOrdinal(name));
        }
    }
}
