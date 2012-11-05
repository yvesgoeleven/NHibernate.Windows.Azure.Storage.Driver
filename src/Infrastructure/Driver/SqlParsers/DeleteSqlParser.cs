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
            var requestParameters = new List<string>();

            var rowKeyIndex = Array.FindIndex(commandTextParts, s => s == "RowKey");
            var rowKeyValueIndex = commandTextParts[rowKeyIndex + 2];
            var rowKeyValue = rowKeyValueIndex == "null" ? null : parameters[int.Parse(rowKeyValueIndex.TrimStart('p'))].Value.ToString();
            requestParameters.Add("RowKey=" + "'" + rowKeyValue + "'");

            var partitionKeyIndex = Array.FindIndex(commandTextParts, s => s == "PartitionKey");
            var partitionKeyValueIndex = commandTextParts[partitionKeyIndex + 2];
            var partitionKeyValue = partitionKeyValueIndex == "null" ? null : parameters[int.Parse(partitionKeyValueIndex.TrimStart('p'))].Value.ToString();
            requestParameters.Add("PartitionKey=" + "'" + partitionKeyValue + "'");

            var uri = String.Format(settings.Uri.AbsoluteUri + "{0}({1})", tableName, String.Join(",", requestParameters.ToArray()));
            var request = new TableStorageRequest(StorageHttpConstants.HttpMethod.Delete, uri, settings) { IfMatch = "*" };
            return request;
        }
    }
}