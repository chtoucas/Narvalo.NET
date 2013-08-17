namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Narvalo;

    public static class SqlCommandExtensions
    {
        public static void AddParameter(
            this SqlCommand command, string parameterName, SqlDbType sqlDbType, object value)
        {
            Requires.Object(command);
            command.Parameters.Add(parameterName, sqlDbType).Value = value;
        }
    }

}