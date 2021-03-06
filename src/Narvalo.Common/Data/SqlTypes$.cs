﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data.SqlTypes;

    using Narvalo.Applicative;

    /// <summary>
    /// Provides extension methods to convert native SQL server data types to CLR types.
    /// </summary>
    public static partial class SqlTypesExtensions
    {
        public static bool? ToNullable(this SqlBoolean @this)
            => @this.IsNull ? (bool?)null : @this.Value;

        public static byte? ToNullable(this SqlByte @this)
            => @this.IsNull ? (byte?)null : @this.Value;

        public static DateTime? ToNullable(this SqlDateTime @this)
            => @this.IsNull ? (DateTime?)null : @this.Value;

        public static decimal? ToNullable(this SqlDecimal @this)
            => @this.IsNull ? (decimal?)null : @this.Value;

        public static double? ToNullable(this SqlDouble @this)
            => @this.IsNull ? (double?)null : @this.Value;

        public static Guid? ToNullable(this SqlGuid @this)
            => @this.IsNull ? (Guid?)null : @this.Value;

        public static short? ToNullable(this SqlInt16 @this)
            => @this.IsNull ? (short?)null : @this.Value;

        public static int? ToNullable(this SqlInt32 @this)
            => @this.IsNull ? (int?)null : @this.Value;

        public static long? ToNullable(this SqlInt64 @this)
            => @this.IsNull ? (long?)null : @this.Value;

        public static decimal? ToNullable(this SqlMoney @this)
            => @this.IsNull ? (decimal?)null : @this.Value;

        public static float? ToNullable(this SqlSingle @this)
            => @this.IsNull ? (float?)null : @this.Value;
    }

    // Extension methods to convert native SQL server data types to CLR reference types.
    public static partial class SqlTypesExtensions
    {
        public static Maybe<byte[]> ToMaybe(this SqlBinary @this)
            => @this.IsNull ? Maybe<byte[]>.None : Maybe.Of(@this.Value);

        public static Maybe<byte[]> ToMaybe(this SqlBytes @this)
            => @this == null || @this.IsNull ? Maybe<byte[]>.None : Maybe.Of(@this.Value);

        public static Maybe<char[]> ToMaybe(this SqlChars @this)
            => @this == null || @this.IsNull ? Maybe<char[]>.None : Maybe.Of(@this.Value);

        public static Maybe<string> ToMaybe(this SqlString @this)
            => @this.IsNull ? Maybe<string>.None : Maybe.Of(@this.Value);

        public static Maybe<string> ToMaybe(this SqlXml @this)
            => @this == null || @this.IsNull ? Maybe<string>.None : Maybe.Of(@this.Value);
    }
}
