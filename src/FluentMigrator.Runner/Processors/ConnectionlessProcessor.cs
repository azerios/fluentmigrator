#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Data;

using FluentMigrator.Expressions;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Initialization;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

namespace FluentMigrator.Runner.Processors
{
    public class ConnectionlessProcessor: IMigrationProcessor
    {
#pragma warning disable 612
        [Obsolete]
        private readonly IMigrationProcessorOptions _legacyOptions;
#pragma warning restore 612

        [Obsolete]
        public ConnectionlessProcessor(
            IMigrationGenerator generator,
            IRunnerContext context,
            IMigrationProcessorOptions options)
        {
            _legacyOptions = options;
            DatabaseType = context.Database;
            Generator = generator;
            Announcer = context.Announcer;
            Options = options.GetProcessorOptions(connectionString: null);
        }

        public ConnectionlessProcessor(
            [NotNull] IGeneratorAccessor generatorAccessor,
            [NotNull] IAnnouncer announcer,
            [NotNull] IOptions<ProcessorOptions> options,
            [NotNull] IOptions<SelectingProcessorAccessorOptions> accessorOptions)
        {
            var generator = generatorAccessor.Generator;
            DatabaseType = string.IsNullOrEmpty(accessorOptions.Value.ProcessorId) ? generator.GetName() : accessorOptions.Value.ProcessorId;
            Generator = generator;
            Announcer = announcer;
            Options = options.Value;
#pragma warning disable 612
            _legacyOptions = options.Value;
#pragma warning restore 612
        }

        [Obsolete("Will change from public to protected")]
        public string ConnectionString { get; } = "No connection";

        public IMigrationGenerator Generator { get; set; }
        public IAnnouncer Announcer { get; set; }
        public ProcessorOptions Options {get; set;}

        [Obsolete]
        IMigrationProcessorOptions IMigrationProcessor.Options => _legacyOptions;

        /// <inheritdoc />
        public void Execute(string sql)
        {
            Process(sql);
        }

        public void Execute(string template, params object[] args)
        {
            Process(string.Format(template, args));
        }

        public DataSet ReadTableData(string schemaName, string tableName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public DataSet Read(string template, params object[] args)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool Exists(string template, params object[] args)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public void BeginTransaction()
        {

        }

        public void CommitTransaction()
        {

        }

        public void RollbackTransaction()
        {

        }

        protected void Process(string sql)
        {
            Announcer.Sql(sql);
        }

        public void Process(CreateSchemaExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteSchemaExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(AlterTableExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(AlterColumnExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(CreateTableExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(CreateColumnExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteTableExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteColumnExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(CreateForeignKeyExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteForeignKeyExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(CreateIndexExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteIndexExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(RenameTableExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(RenameColumnExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(InsertDataExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(AlterDefaultConstraintExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(PerformDBOperationExpression expression)
        {
            Announcer.Say("Performing DB Operation");
        }

        public void Process(DeleteDataExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(UpdateDataExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(AlterSchemaExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(CreateSequenceExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteSequenceExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(CreateConstraintExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteConstraintExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public void Process(DeleteDefaultConstraintExpression expression)
        {
            Process(Generator.Generate(expression));
        }

        public bool SchemaExists(string schemaName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool TableExists(string schemaName, string tableName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool ColumnExists(string schemaName, string tableName, string columnName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool ConstraintExists(string schemaName, string tableName, string constraintName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool IndexExists(string schemaName, string tableName, string indexName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool SequenceExists(string schemaName, string sequenceName)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

        public bool DefaultValueExists(string schemaName, string tableName, string columnName, object defaultValue)
        {
            throw new NotImplementedException("Method is not supported by the connectionless processor");
        }

#pragma warning disable 618
        public string DatabaseType { get; }
#pragma warning restore 618

        public IList<string> DatabaseTypeAliases { get; } = new List<string>();

        public void Dispose()
        {
        }
    }
}
