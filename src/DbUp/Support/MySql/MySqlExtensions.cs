using System;
using System.Data;
using System.Data.SqlClient;
using DbUp.Builder;
using DbUp.Support.SqlServer;

namespace DbUp.Support.MySql
{
    /// <summary>
    /// Configuration extension methods for MySql Server.
    /// </summary>
// ReSharper disable CheckNamespace
    public static class MySqlExtensions
// ReSharper restore CheckNamespace
    {
        /// <summary>
        /// Creates an upgrader for SQL Server databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// A builder for a database upgrader designed for SQL Server databases.
        /// </returns>
        public static UpgradeEngineBuilder MySqlDatabase(this SupportedDatabases supported, string connectionString)
        {
            return MySqlDatabase(supported, connectionString, null);
        }

        /// <summary>
        /// Creates an upgrader for SQL Server databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="schema">The SQL schema name to use. Defaults to 'dbo'.</param>
        /// <returns>
        /// A builder for a database upgrader designed for SQL Server databases.
        /// </returns>
        public static UpgradeEngineBuilder MySqlDatabase(this SupportedDatabases supported, string connectionString, string schema)
        {
            return MySqlDatabase(supported, () => new SqlConnection(connectionString), schema);
        }

        /// <summary>
        /// Creates an upgrader for SQL Server databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <returns>
        /// A builder for a database upgrader designed for SQL Server databases.
        /// </returns>
        public static UpgradeEngineBuilder MySqlDatabase(this SupportedDatabases supported, Func<IDbConnection> connectionFactory)
        {
            return MySqlDatabase(supported, connectionFactory, null);
        }

        /// <summary>
        /// Creates an upgrader for SQL Server databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="schema">The SQL schema name to use. Defaults to 'dbo'.</param>
        /// <returns>
        /// A builder for a database upgrader designed for SQL Server databases.
        /// </returns>
        public static UpgradeEngineBuilder MySqlDatabase(this SupportedDatabases supported, Func<IDbConnection> connectionFactory, string schema)
        {
            var builder = new UpgradeEngineBuilder();
            builder.Configure(c => c.ConnectionFactory = connectionFactory);
            builder.Configure(c => c.ScriptExecutor = new SqlScriptExecutor(c.ConnectionFactory, () => c.Log, schema, () => c.VariablesEnabled, c.ScriptPreprocessors));
            builder.Configure(c => c.Journal = new MySqlTableJournal(c.ConnectionFactory, schema, "SchemaVersions", c.Log));
            return builder;
        }

        /// <summary>
        /// Tracks the list of executed scripts in a SQL Server table.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static UpgradeEngineBuilder JournalToSqlTable(this UpgradeEngineBuilder builder, string schema, string table)
        {
            builder.Configure(c => c.Journal = new MySqlTableJournal(c.ConnectionFactory, schema, table, c.Log));
            return builder;
        }
    }
}