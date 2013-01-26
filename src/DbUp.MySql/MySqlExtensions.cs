using System;
using DbUp.Builder;
using DbUp.MySql;
using DbUp.Support.MySql;
using DbUp.Support.SqlServer;
using MySql.Data.MySqlClient;

public static class MySqlExtensions
{
    /// <summary>
    /// Creates an upgrader for MySql databases.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <returns>
    /// A builder for a database upgrader designed for MySql databases.
    /// </returns>
    public static UpgradeEngineBuilder MySqlDatabase(this SupportedDatabases supported, Func<MySqlConnection> connectionFactory)
    {
        var builder = new UpgradeEngineBuilder();
        builder.Configure(c => c.ConnectionFactory = connectionFactory);
        builder.Configure(c => c.ScriptExecutor = new MySqlScriptExecutor(c.ConnectionFactory, () => c.Log, null, () => c.VariablesEnabled, c.ScriptPreprocessors));
        builder.Configure(c => c.Journal = new MySqlTableJournal(c.ConnectionFactory, null, "SchemaVersions", c.Log));
        builder.WithPreprocessor(new MySqlPreprocessor());
        return builder;
    }

    /// <summary>
    /// Creates an upgrader for MySql databases.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>
    /// A builder for a database upgrader designed for MySql databases.
    /// </returns>
    public static UpgradeEngineBuilder SqlCeDatabase(this SupportedDatabases supported, string connectionString)
    {
        return supported.MySqlDatabase(() => new MySqlConnection(connectionString));
    }
}