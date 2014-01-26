namespace Narvalo.Data
{
    using System;
    using System.Data.SqlTypes;
    using Narvalo.Fx;

    /// <summary>
    /// Fournit des méthodes d'extension pour les types de données natifs SQL Server.
    /// </summary>
    public static class SqlTypesExtensions
    {
        public static Maybe<byte[]> ToMaybe(this SqlBinary @this)
        {
            return @this.IsNull ? Maybe<byte[]>.None : Maybe.Create(@this.Value);
        }

        //public static Maybe<bool> ToMaybe(this SqlBoolean @this)
        //{
        //    return @this.IsNull ? Maybe<bool>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<byte> ToMaybe(this SqlByte @this)
        //{
        //    return @this.IsNull ? Maybe<byte>.None : Maybe.Create(@this.Value);
        //}

        public static Maybe<byte[]> ToMaybe(this SqlBytes @this)
        {
            Require.Object(@this);

            return @this.IsNull ? Maybe<byte[]>.None : Maybe.Create(@this.Value);
        }

        public static Maybe<char[]> ToMaybe(this SqlChars @this)
        {
            Require.Object(@this);

            return @this.IsNull ? Maybe<char[]>.None : Maybe.Create(@this.Value);
        }

        //public static Maybe<DateTime> ToMaybe(this SqlDateTime @this)
        //{
        //    return @this.IsNull ? Maybe<DateTime>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<decimal> ToMaybe(this SqlDecimal @this)
        //{
        //    return @this.IsNull ? Maybe<decimal>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<double> ToMaybe(this SqlDouble @this)
        //{
        //    return @this.IsNull ? Maybe<double>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<Guid> ToMaybe(this SqlGuid @this)
        //{
        //    return @this.IsNull ? Maybe<Guid>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<short> ToMaybe(this SqlInt16 @this)
        //{
        //    return @this.IsNull ? Maybe<short>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<int> ToMaybe(this SqlInt32 @this)
        //{
        //    return @this.IsNull ? Maybe<int>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<long> ToMaybe(this SqlInt64 @this)
        //{
        //    return @this.IsNull ? Maybe<long>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<decimal> ToMaybe(this SqlMoney @this)
        //{
        //    return @this.IsNull ? Maybe<decimal>.None : Maybe.Create(@this.Value);
        //}

        //public static Maybe<float> ToMaybe(this SqlSingle @this)
        //{
        //    return @this.IsNull ? Maybe<float>.None : Maybe.Create(@this.Value);
        //}

        public static Maybe<string> ToMaybe(this SqlString @this)
        {
            return @this.IsNull ? Maybe<string>.None : Maybe.Create(@this.Value);
        }

        public static Maybe<string> ToMaybe(this SqlXml @this)
        {
            Require.Object(@this);

            return @this.IsNull ? Maybe<string>.None : Maybe.Create(@this.Value);
        }
    }
}
