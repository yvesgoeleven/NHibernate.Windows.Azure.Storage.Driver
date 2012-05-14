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

            for (var i = whereIndex + 1; i < commandTextParts.Length; i += 4)
            {
                var part = commandTextParts[i];
                var value = parameters[(i - 3)/3].Value.ToString();
                requestParameters.Add( part + "=" + "'" + value + "'");

                if (part == "RowKey") rowKey = value;
                if (part == "PartitionKey") partitionKey = value;
            }

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