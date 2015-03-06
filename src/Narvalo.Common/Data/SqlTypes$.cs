// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data.SqlTypes;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for native SQL server data types.
    /// </summary>
    public static class SqlTypesExtensions
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
