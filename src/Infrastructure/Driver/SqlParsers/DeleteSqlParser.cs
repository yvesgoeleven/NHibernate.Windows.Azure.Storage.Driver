using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses delete sql statements into a table storage request
    /// </summary>
    public class DeleteSqlParser : ISqlParser
    {
        private readonly string[] commandTextParts;
        private readonly string tableName;

        public DeleteSqlParser(string commandText)
        {
            commandTextParts = commandText.Split(new[] { ' ', '(', ',', ')', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            tableName = commandTextParts[2];
        }

        /// <summary>
        /// Gets the table storage request for the given settings and values
        /// </summary>
        public TableStorageRequest GetTableStorageRequest(TableStorageSettings settings, DbParameterCollection parameters)
        {
            var parameterNames = new string[parameters.Count];
            for (var i = 0; i < parameterNames.Length; i++)
            {
                parameterNames[i] = commandTextParts[(i + 1) * 4];
            }

            var uri = String.Format(settings.Uri.AbsoluteUri + "{0}({1})", tableName, String.Join(",", parameterNames.Select((t, i) => t + "=" + "'" + parameters[i].Value + "'").ToArray()));
            var request = new TableStorageRequest(StorageHttpConstants.HttpMethod.Delete, uri, settings) { IfMatch = "*" };
            return request;
        }
    }
}