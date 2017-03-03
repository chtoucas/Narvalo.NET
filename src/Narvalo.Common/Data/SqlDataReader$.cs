// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    /*!
     * Méthodes d'extension pour `SqlDataReader`
     * =========================================
     *
     * TODO: Mettre à jour!
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
    /// Provides extension methods for <see cref="SqlDataReader"/>.
    /// </summary>
    public static partial class SqlDataReaderExtensions
    {
        public static bool GetBoolean(this SqlDataReader @this, int ordinal, bool defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlBoolean(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static bool GetBoolean(this SqlDataReader @this, string name, bool defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetBoolean(@this.GetOrdinal(name), defaultValue);
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlBoolean(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static bool? GetNullableBoolean(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableBoolean(@this.GetOrdinal(name));
        }

        public static byte GetByte(this SqlDataReader @this, int ordinal, byte defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlByte(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static byte GetByte(this SqlDataReader @this, string name, byte defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetByte(@this.GetOrdinal(name), defaultValue);
        }

        public static byte? GetNullableByte(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlByte(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static byte? GetNullableByte(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableByte(@this.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this SqlDataReader @this, int ordinal, DateTime defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlDateTime(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static DateTime GetDateTime(this SqlDataReader @this, string name, DateTime defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDateTime(@this.GetOrdinal(name), defaultValue);
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlDateTime(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableDateTime(@this.GetOrdinal(name));
        }

        public static decimal GetDecimal(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlDecimal(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetDecimal(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDecimal(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlDecimal(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableDecimal(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableDecimal(@this.GetOrdinal(name));
        }

        public static double GetDouble(this SqlDataReader @this, int ordinal, double defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlDouble(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static double GetDouble(this SqlDataReader @this, string name, double defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetDouble(@this.GetOrdinal(name), defaultValue);
        }

        public static double? GetNullableDouble(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlDouble(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static double? GetNullableDouble(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableDouble(@this.GetOrdinal(name));
        }

        public static Guid GetGuid(this SqlDataReader @this, int ordinal, Guid defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlGuid(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static Guid GetGuid(this SqlDataReader @this, string name, Guid defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetGuid(@this.GetOrdinal(name), defaultValue);
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlGuid(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static Guid? GetNullableGuid(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableGuid(@this.GetOrdinal(name));
        }

        public static short GetInt16(this SqlDataReader @this, int ordinal, short defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlInt16(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static short GetInt16(this SqlDataReader @this, string name, short defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt16(@this.GetOrdinal(name), defaultValue);
        }

        public static short? GetNullableInt16(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlInt16(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static short? GetNullableInt16(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableInt16(@this.GetOrdinal(name));
        }

        public static int GetInt32(this SqlDataReader @this, int ordinal, int defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlInt32(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static int GetInt32(this SqlDataReader @this, string name, int defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt32(@this.GetOrdinal(name), defaultValue);
        }

        public static int? GetNullableInt32(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlInt32(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static int? GetNullableInt32(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableInt32(@this.GetOrdinal(name));
        }

        public static long GetInt64(this SqlDataReader @this, int ordinal, long defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlInt64(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static long GetInt64(this SqlDataReader @this, string name, long defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetInt64(@this.GetOrdinal(name), defaultValue);
        }

        public static long? GetNullableInt64(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            var value = @this.GetSqlInt64(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static long? GetNullableInt64(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableInt64(@this.GetOrdinal(name));
        }
    }

    // Extensions for reference types.
    public static partial class SqlDataReaderExtensions
    {
        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetBytesUnchecked(ordinal);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<byte[]> MayGetBytesUnchecked(this SqlDataReader @this, int ordinal)
            => @this.GetSqlBytes(ordinal).ToMaybe();

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetBytesUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<byte[]> MayGetBytesUnchecked(this SqlDataReader @this, string name)
            => @this.MayGetBytes(@this.GetOrdinal(name));

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetBinaryUnchecked(ordinal);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<byte[]> MayGetBinaryUnchecked(this SqlDataReader @this, int ordinal)
            => @this.GetSqlBinary(ordinal).ToMaybe();

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetBinaryUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<byte[]> MayGetBinaryUnchecked(this SqlDataReader @this, string name)
            => @this.MayGetBinary(@this.GetOrdinal(name));

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetCharsUnchecked(ordinal);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<char[]> MayGetCharsUnchecked(this SqlDataReader @this, int ordinal)
            => @this.GetSqlChars(ordinal).ToMaybe();

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetCharsUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<char[]> MayGetCharsUnchecked(this SqlDataReader @this, string name)
            => @this.MayGetChars(@this.GetOrdinal(name));

        public static string GetString(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetStringUnchecked(ordinal, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static string GetStringUnchecked(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            var value = @this.GetSqlString(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetString(this SqlDataReader @this, string name, string defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetStringUnchecked(name, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static string GetStringUnchecked(this SqlDataReader @this, string name, string defaultValue)
        {
            return @this.GetString(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetStringUnchecked(ordinal);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<string> MayGetStringUnchecked(this SqlDataReader @this, int ordinal)
            => @this.GetSqlString(ordinal).ToMaybe();

        public static Maybe<string> MayGetString(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetStringUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<string> MayGetStringUnchecked(this SqlDataReader @this, string name)
            => @this.MayGetString(@this.GetOrdinal(name));

        public static decimal GetMoney(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetMoneyUnchecked(ordinal, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static decimal GetMoneyUnchecked(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            var value = @this.GetSqlMoney(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetMoney(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetMoneyUnchecked(name, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static decimal GetMoneyUnchecked(this SqlDataReader @this, string name, decimal defaultValue)
            => @this.GetMoney(@this.GetOrdinal(name), defaultValue);

        public static decimal? GetNullableMoney(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableMoneyUnchecked(ordinal);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static decimal? GetNullableMoneyUnchecked(this SqlDataReader @this, int ordinal)
        {
            var value = @this.GetSqlMoney(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetNullableMoneyUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static decimal? GetNullableMoneyUnchecked(this SqlDataReader @this, string name)
            => @this.GetNullableMoney(@this.GetOrdinal(name));

        public static string GetXml(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetXmlUnchecked(ordinal, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static string GetXmlUnchecked(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            var value = @this.GetSqlXml(ordinal);

            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetXml(this SqlDataReader @this, string name, string defaultValue)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.GetXmlUnchecked(name, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static string GetXmlUnchecked(this SqlDataReader @this, string name, string defaultValue)
            => @this.GetXml(@this.GetOrdinal(name), defaultValue);

        public static Maybe<string> MayGetXml(this SqlDataReader @this, int ordinal)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetXmlUnchecked(ordinal);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<string> MayGetXmlUnchecked(this SqlDataReader @this, int ordinal)
            => @this.GetSqlXml(ordinal).ToMaybe();

        public static Maybe<string> MayGetXml(this SqlDataReader @this, string name)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.MayGetXmlUnchecked(name);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0",
            Justification = "[Intentionally] This method clearly states that the responsibility for null-checks is on the callers.")]
        public static Maybe<string> MayGetXmlUnchecked(this SqlDataReader @this, string name)
            => @this.MayGetXml(@this.GetOrdinal(name));
    }
}
