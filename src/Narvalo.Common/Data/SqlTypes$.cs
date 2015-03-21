// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data.SqlTypes;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods to convert native SQL server data types to CLR types.
    /// </summary>
    public static partial class SqlTypesExtensions
    {
        public static bool? ToNullable(this SqlBoolean @this)
        {
            return @this.IsNull ? (bool?)null : @this.Value;
        }

        public static byte? ToNullable(this SqlByte @this)
        {
            return @this.IsNull ? (byte?)null : @this.Value;
        }

        public static DateTime? ToNullable(this SqlDateTime @this)
        {
            return @this.IsNull ? (DateTime?)null : @this.Value;
        }

        public static decimal? ToNullable(this SqlDecimal @this)
        {
            return @this.IsNull ? (decimal?)null : @this.Value;
        }

        public static double? ToNullable(this SqlDouble @this)
        {
            return @this.IsNull ? (double?)null : @this.Value;
        }

        public static Guid? ToNullable(this SqlGuid @this)
        {
            return @this.IsNull ? (Guid?)null : @this.Value;
        }

        public static short? ToNullable(this SqlInt16 @this)
        {
            return @this.IsNull ? (short?)null : @this.Value;
        }

        public static int? ToNullable(this SqlInt32 @this)
        {
            return @this.IsNull ? (int?)null : @this.Value;
        }

        public static long? ToNullable(this SqlInt64 @this)
        {
            return @this.IsNull ? (long?)null : @this.Value;
        }

        public static decimal? ToNullable(this SqlMoney @this)
        {
            return @this.IsNull ? (decimal?)null : @this.Value;
        }

        public static float? ToNullable(this SqlSingle @this)
        {
            return @this.IsNull ? (float?)null : @this.Value;
        }
    }

    /// <content>
    /// Implements extension methods to convert native SQL server data types to CLR reference types.
    /// </content>
    public static partial class SqlTypesExtensions
    {
        public static Maybe<byte[]> ToMaybe(this SqlBinary @this)
        {
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this.IsNull ? Maybe<byte[]>.None : Maybe.Create(@this.Value);
        }

        public static Maybe<byte[]> ToMaybe(this SqlBytes @this)
        {
            Contract.Ensures(Contract.Result<Maybe<byte[]>>() != null);

            return @this == null || @this.IsNull ? Maybe<byte[]>.None : Maybe.Create(@this.Value);
        }

        public static Maybe<char[]> ToMaybe(this SqlChars @this)
        {
            Contract.Ensures(Contract.Result<Maybe<char[]>>() != null);

            return @this == null || @this.IsNull ? Maybe<char[]>.None : Maybe.Create(@this.Value);
        }

        public static Maybe<string> ToMaybe(this SqlString @this)
        {
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this.IsNull ? Maybe<string>.None : Maybe.Create(@this.Value);
        }

        public static Maybe<string> ToMaybe(this SqlXml @this)
        {
            Contract.Ensures(Contract.Result<Maybe<string>>() != null);

            return @this == null || @this.IsNull ? Maybe<string>.None : Maybe.Create(@this.Value);
        }
    }
}
