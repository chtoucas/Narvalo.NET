namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Narvalo;

    public abstract class NonQueryStoredProcedure<TParameters>
    {
        readonly string _connectionString;
        readonly string _name;

        public NonQueryStoredProcedure(string connectionString, string name)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(name, "name");

            _connectionString = connectionString;
            _name = name;
        }

        protected string ConnectionString { get { return _connectionString; } }
        protected string Name { get { return _name; } }

        public int Execute(TParameters parameters)
        {
            int result;

            using (var connection = CreateConnection()) {
                using (var command = CreateCommand(connection)) {
                    PrepareCommand(command, parameters);

                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        protected virtual void PrepareCommand(SqlCommand command, TParameters parameters) { ; }

        protected virtual SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected virtual SqlCommand CreateCommand(SqlConnection connection)
        {
            return new SqlCommand(Name, connection) { CommandType = CommandType.StoredProcedure };
        }
    }
}
