using System;
using System.Collections.Generic;
using System.Data.Common;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses update sql statements into a table storage request
    /// </summary>
    public class UpdateSqlParser : ISqlParser
    {
        private readonly string tableName;
        private readonly int setIndex;
        private readonly int whereIndex;
        private readonly string[] commandTextParts;

        public UpdateSqlParser(string commandText, ETagTracker tracker)
        {
            Tracker = tracker;

            commandTextParts = commandText.Split(new[] { " ", "(", ",", ")", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            tableName = commandTextParts[1];
            setIndex = Array.FindIndex(commandTextParts, s => s == "SET");
            whereIndex = Array.FindIndex(commandTextParts, s => s == "WHERE");
        }

        /// <summary>
        /// Gets or sets the ETag tracker
        /// </summary>
        public ETagTracker Tracker { get; private set; }

        /// <summary>
        /// Gets the table storage request for the given settings and values
        /// </summary>
        public TableStorageRequest GetTableStorageRequest(TableStorageSettings settings, DbParameterCollection parameters)
        {
            var rowKey = string.Empty;
            var partitionKey = string.Empty;
            var requestParameters = new List<string>();
            var bodyParameters = new List<string>();

            for (var i = setIndex + 1; i < whereIndex; i += 3)
            {
                bodyParameters.Add(commandTextParts[i]);
            }

            var rowKeyIndex = Array.FindIndex(commandTextParts, s => s == "RowKey");
            var rowKeyValueIndex = commandTextParts[rowKeyIndex + 2];
            var rowKeyValue = rowKeyValueIndex == "null" ? null : parameters[int.Parse(rowKeyValueIndex.TrimStart('p'))].Value.ToString();
            requestParameters.Add("RowKey=" + "'" + rowKeyValue + "'");
            rowKey = rowKeyValue;

            var partitionKeyIndex = Array.FindIndex(commandTextParts, s => s == "PartitionKey");
            var partitionKeyValueIndex = commandTextParts[partitionKeyIndex + 2];
            var partitionKeyValue = partitionKeyValueIndex == "null" ? null : parameters[int.Parse(partitionKeyValueIndex.TrimStart('p'))].Value.ToString();
            requestParameters.Add("PartitionKey=" + "'" + partitionKeyValue + "'");
            partitionKey = partitionKeyValue;

            var uri = String.Format(settings.Uri.AbsoluteUri + "{0}({1})", tableName, String.Join(",", requestParameters.ToArray()));
            
            var request = new TableStorageRequest(StorageHttpConstants.HttpMethod.Merge, uri, settings) { IfMatch = Tracker.GetIfMatchHeaderFor(tableName, rowKey, partitionKey) };
            for (var i = 0; i < bodyParameters.Count; i++)
            {
                request.Body.AddProperty(bodyParameters[i], parameters[i].Value, parameters[i].DbType);
            }
            return request;
        }
    }
}