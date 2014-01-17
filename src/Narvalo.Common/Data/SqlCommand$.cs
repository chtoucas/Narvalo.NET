namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Data.SqlClient.SqlCommand"/>.
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