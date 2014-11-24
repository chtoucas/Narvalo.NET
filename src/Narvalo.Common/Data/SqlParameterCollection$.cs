// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Provides extension methods for <see cref="System.Data.SqlClient.SqlParameterCollection"/>.
    /// </summary>
    public static class SqlParameterCollectionExtensions
    {
        public static void AddParameter(
            this SqlParameterCollection @this, string parameterName, SqlDbType parameterType, object value)
        {
            Require.Object(@this);

            @this.Add(parameterName, parameterType).AssumeNotNull().Value = value;
        }
    }
}