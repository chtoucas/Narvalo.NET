// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Provides extension methods for <see cref="System.Data.SqlClient.SqlCommand"/>.
    /// </summary>
    public static class SqlCommandExtensions
    {
        public static void AddParameter(
            this SqlCommand @this, string parameterName, SqlDbType parameterType, object value)
        {
            Require.Object(@this);

            @this.Parameters.AssumeNotNull()
                .Add(parameterName, parameterType).AssumeNotNull()
                .Value = value;
        }
    }
}