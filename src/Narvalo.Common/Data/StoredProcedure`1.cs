namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Narvalo;

    public abstract class StoredProcedure<TResult>
    {
        readonly string _connectionString;
        readonly string _name;

        CommandBehavior _commandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleResult;

        protected StoredProcedure(string connectionString, string name)
        {
            Requires.NotNull(connectionString, "connectionString");
            Requires.NotNull(name, "name");

            _connectionString = connectionString;
            _name = name;
        }

        protected CommandBehavior CommandBehavior
        {
            get { return _commandBehavior; }
            set { _commandBehavior = value; }
        }

        protected string ConnectionString { get { return _connectionString; } }
        protected string Name { get { return _name; } }

        protected abstract TResult Execute(SqlDataReader rdr);

        public TResult Execute()
        {
            TResult result;

            using (var connection = CreateConnection()) {
                using (var command = CreateCommand(connection)) {
                    PrepareCommand(command);

                    connection.Open();

                    using (var reader = ExecuteCommand(command)) {
                        result = Execute(reader);
                    }
                }
            }

            return result;
        }

        protected virtual void PrepareCommand(SqlCommand command) { ; }

        protected virtual SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected virtual SqlCommand CreateCommand(SqlConnection connection)
        {
            return new SqlCommand(Name, connection) { CommandType = CommandType.StoredProcedure };
        }

        protected SqlDataReader ExecuteCommand(SqlCommand command)
        {
            return command.ExecuteReader(CommandBehavior);
        }
    }
}