#region License
// Copyright (c) 2007-2018, Sean Chambers and the FluentMigrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Collections.Generic;
using System.Linq;

using FluentMigrator.Exceptions;
using FluentMigrator.Infrastructure.Extensions;
using FluentMigrator.Model;
using FluentMigrator.Runner.Generators.Base;
using FluentMigrator.Runner.Processors.Snowflake;
using FluentMigrator.Snowflake;

using JetBrains.Annotations;

namespace FluentMigrator.Runner.Generators.Snowflake
{
    internal class SnowflakeColumn : ColumnBase
    {
        public SnowflakeColumn([NotNull] SnowflakeOptions sfOptions) : base(
            new SnowflakeTypeMap(),
            new SnowflakeQuoter(sfOptions.QuoteIdentifiers))
        {
        }

        internal string GenerateAlterColumn(ColumnDefinition column)
        {
            var clauses = new List<string>();
            var dropOrSet = column.IsNullable ?? false ? "DROP" : "SET";
            var setNullableClause = $"COLUMN {FormatString(column)} {dropOrSet} NOT NULL";
            clauses.Add(setNullableClause);
            var typeClause = $"COLUMN {FormatString(column)} {FormatType(column)}";
            clauses.Add(typeClause);
            var commentClause = string.Empty;
            if (column.ColumnDescriptions.Values.Count == 0)
            {
                commentClause = $"COLUMN {FormatString(column)} COMMENT ''";
            }
            else
            {
                var descriptionsList = new List<string>();
                foreach (var description in column.ColumnDescriptions)
                {
                    var newDescriptionStatement = description.Key + ":" + description.Value;
                    descriptionsList.Add(newDescriptionStatement);
                }

                var multipleComments = string.Empty;
                if (descriptionsList.Count == 1)
                {
                    multipleComments = descriptionsList.First();
                }
                else
                {
                    multipleComments = string.Join("\r\n", descriptionsList);
                }
                     

                commentClause = $"COLUMN {FormatString(column)} COMMENT '" + multipleComments + "'";
            }
            clauses.Add(commentClause); 

            return string.Join(", ", clauses);
        }

        /// <inheritdoc />
        protected override string FormatIdentity(ColumnDefinition column)
        {
            return column.IsIdentity ? GetIdentityString(column) : string.Empty;
        }

        /// <inheritdoc />
        protected override string FormatCollation(ColumnDefinition column)
        {
            if (!string.IsNullOrEmpty(column.CollationName))
            {
                throw new DatabaseOperationNotSupportedException("Snowflake database does not support collation.");
            }

            return string.Empty;
        }

        private static string GetIdentityString(ColumnDefinition column)
        {
            return $"IDENTITY({column.GetAdditionalFeature(SnowflakeExtensions.IdentitySeed, 1)},{column.GetAdditionalFeature(SnowflakeExtensions.IdentityIncrement, 1)})";
        }
    }
}
