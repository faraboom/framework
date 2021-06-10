namespace Faraboom.Framework.DataAccess.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;

    public class Migrator : IMigrator
    {
        private readonly IHistoryRepository historyRepository;
        private readonly IRawSqlCommandBuilder rawSqlCommandBuilder;
        private readonly IMigrationCommandExecutor migrationCommandExecutor;
        private readonly IRelationalConnection connection;
        private readonly ICurrentDbContext currentContext;
        private readonly ILogger<Migrator> logger;
        private readonly IDiagnosticsLogger<DbLoggerCategory.Migrations> migrationsLogger;
        private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger;

        public Migrator(
            IHistoryRepository historyRepository,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IMigrationCommandExecutor migrationCommandExecutor,
            IRelationalConnection connection,
            ICurrentDbContext currentContext,
            ILoggerFactory loggerFactory,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> migrationsLogger,
            IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
        {
            this.rawSqlCommandBuilder = rawSqlCommandBuilder;
            this.migrationCommandExecutor = migrationCommandExecutor;
            this.connection = connection;
            this.currentContext = currentContext;
            logger = loggerFactory.CreateLogger<Migrator>();
            this.migrationsLogger = migrationsLogger;
            this.commandLogger = commandLogger;
            this.historyRepository = historyRepository;

            var factory = Data.DbProviderFactories.GetFactory;
            this.historyRepository.GetType().BaseType?.BaseType?.GetField("_migrationIdColumnName", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)?.SetValue(this.historyRepository, factory.GetObjectName(nameof(HistoryRow.MigrationId), pluralize: false));
            this.historyRepository.GetType().BaseType?.BaseType?.GetField("_productVersionColumnName", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)?.SetValue(this.historyRepository, factory.GetObjectName(nameof(HistoryRow.ProductVersion), pluralize: false));
        }

        public async Task MigrateAsync(string targetMigration = null, CancellationToken cancellationToken = default)
        {
            migrationsLogger.MigrateUsingConnection(this, connection);

            var migrationsToApply = await GetMigrationsToApplyAsync(cancellationToken);
            if (migrationsToApply?.Any() == true)
            {
                var schema = currentContext.Context.Model.GetDefaultSchema();
                for (int i = 0; i < migrationsToApply.Count; i++)
                {
                    var migration = migrationsToApply[i];
                    var migrationCommands = migration.Query.Replace(Core.Constants.SchemaIdentifier, schema).Split(";", StringSplitOptions.RemoveEmptyEntries);
                    if (migrationCommands.Length > 0)
                    {
                        var breaked = false;
                        foreach (var item in migrationCommands)
                        {
                            var cmd = new MigrationCommand(rawSqlCommandBuilder.Build(item), currentContext.Context, commandLogger);
                            try
                            {
                                await migrationCommandExecutor.ExecuteNonQueryAsync(new[] { cmd }, connection, cancellationToken);
                            }
                            catch (Exception exc)
                            {
                                logger.LogError(exc, "Command: {Command}, Version: {Version}", cmd, migration.Version);
                                breaked = true;
                                break;
                            }
                        }

                        if (breaked)
                        {
                            break;
                        }

                        var historySql = Prepare(historyRepository.GetInsertScript(new HistoryRow(migration.Version, migration.Version)));
                        var historyCmd = new MigrationCommand(rawSqlCommandBuilder.Build(historySql), currentContext.Context, commandLogger);
                        await migrationCommandExecutor.ExecuteNonQueryAsync(new[] { historyCmd }, connection, cancellationToken);
                    }
                }
            }
        }

        public string GenerateScript(string fromMigration = null, string toMigration = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        {
            throw new NotImplementedException();
        }

        public void Migrate(string targetMigration = null)
        {
            throw new NotImplementedException();
        }

        private static string Prepare(string sql)
        {
            var factory = Data.DbProviderFactories.GetFactory;
            return sql
                .Replace(nameof(HistoryRow.MigrationId), factory.GetObjectName(nameof(HistoryRow.MigrationId), pluralize: false))
                    .Replace(nameof(HistoryRow.ProductVersion), factory.GetObjectName(nameof(HistoryRow.ProductVersion), pluralize: false));
        }

        private async Task<IReadOnlyList<(string Version, string Query)>> GetMigrationsToApplyAsync(CancellationToken cancellationToken = default)
        {
            var migrationDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Migrations");
            if (!Directory.Exists(migrationDirectory))
            {
                return null;
            }

            if (Directory.GetFiles(migrationDirectory)?.All(t => Path.GetExtension(t).ToUpper() != ".SQL") == true)
            {
                return null;
            }

            if (!await historyRepository.ExistsAsync(cancellationToken))
            {
                var command = rawSqlCommandBuilder.Build(Prepare(historyRepository.GetCreateScript()));

                await command.ExecuteNonQueryAsync(
                    new RelationalCommandParameterObject(
                        connection,
                        null,
                        null,
                        currentContext.Context,
                        commandLogger),
                    cancellationToken);
            }

            var appliedMigrationEntries = await historyRepository.GetAppliedMigrationsAsync(cancellationToken);
            var lastVersion = appliedMigrationEntries.Select(t => int.Parse(t.MigrationId.Replace(".", string.Empty))).OrderByDescending(t => t).FirstOrDefault();

            var lst = Directory.GetFiles(migrationDirectory)
                .Where(t => Path.GetExtension(t).ToUpper() == ".SQL" && int.Parse(Path.GetFileNameWithoutExtension(t).Replace(".", string.Empty)) > lastVersion)
                .OrderBy(t => t)
                .Select(t => t);

            List<(string Version, string Query)> result = new();
            foreach (var item in lst)
            {
                var file = await File.ReadAllTextAsync(item, System.Text.Encoding.UTF8, cancellationToken);
                result.Add((Path.GetFileNameWithoutExtension(item), file));
            }

            return result;
        }
    }
}
