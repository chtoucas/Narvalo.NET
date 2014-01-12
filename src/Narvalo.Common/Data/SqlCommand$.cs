namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Narvalo;

    public static class SqlCommandExtensions
    {
        public static void AddParameter(
            this SqlCommand @this, string parameterName, SqlDbType sqlDbType, object value)
        {
            Requires.Object(@this);

            @this.Parameters.Add(parameterName, sqlDbType).Value = value;
        }
    }

}