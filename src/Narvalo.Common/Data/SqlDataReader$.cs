// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx;

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
        //// Byte[]

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.GetSqlBytes(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBytes(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.MayGetBytes(@this.GetOrdinal(name));
        }

        //// Binary

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.GetSqlBinary(ordinal).ToMaybe();
        }

        public static Maybe<byte[]> MayGetBinary(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.MayGetBinary(@this.GetOrdinal(name));
        }

        //// Char[]

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<char[]>>() != null);

            return @this.GetSqlChars(ordinal).ToMaybe();
        }

        public static Maybe<char[]> MayGetChars(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<char[]>>() != null);

            return @this.MayGetChars(@this.GetOrdinal(name));
        }

        //// String

        public static string GetString(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetString(@this.GetOrdinal(name));
        }

        public static string GetString(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlString(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetString(this SqlDataReader @this, string name, string defaultValue)
        {
            Require.Object(@this);

            return @this.GetString(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.GetSqlString(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetString(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.MayGetString(@this.GetOrdinal(name));
        }

        //// Money

        public static decimal GetMoney(this SqlDataReader @this, int ordinal, decimal defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlMoney(ordinal);
            return value.IsNull ? defaultValue : value.Value;
        }

        public static decimal GetMoney(this SqlDataReader @this, string name, decimal defaultValue)
        {
            Require.Object(@this);

            return @this.GetMoney(@this.GetOrdinal(name), defaultValue);
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlMoney(ordinal);
            if (value.IsNull) { return null; }
            else { return value.Value; }
        }

        public static decimal? GetNullableMoney(this SqlDataReader @this, string name)
        {
            Require.Object(@this);

            return @this.GetNullableMoney(@this.GetOrdinal(name));
        }

        //// Xml

        public static string GetXml(this SqlDataReader @this, int ordinal, string defaultValue)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);

            var value = @this.GetSqlXml(ordinal).AssumeNotNull();
            return value.IsNull ? defaultValue : value.Value;
        }

        public static string GetXml(this SqlDataReader @this, string name, string defaultValue)
        {
            Require.Object(@this);

            return @this.GetXml(@this.GetOrdinal(name), defaultValue);
        }

        public static Maybe<string> MayGetXml(this SqlDataReader @this, int ordinal)
        {
            Require.Object(@this);
            Contract.Requires(ordinal >= 0);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.GetSqlXml(ordinal).ToMaybe();
        }

        public static Maybe<string> MayGetXml(this SqlDataReader @this, string name)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.MayGetXml(@this.GetOrdinal(name));
        }
    }
}
