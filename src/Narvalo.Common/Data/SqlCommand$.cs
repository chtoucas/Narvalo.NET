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

            @this.Parameters.Add(parameterName, parameterType).Value = value;
        }
    }
}