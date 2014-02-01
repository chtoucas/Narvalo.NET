namespace Narvalo.Data
{
    using System.Data.SqlTypes;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for native SQL server data types.
    /// </summary>
    public static class SqlTypesExtensions
    {
        public static Maybe<byte[]> ToMaybe(this SqlBinary @this)
        {
            return @this.IsNull ? Maybe<byte[]>.None : Maybe.Create(@this.Value);
        }

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
