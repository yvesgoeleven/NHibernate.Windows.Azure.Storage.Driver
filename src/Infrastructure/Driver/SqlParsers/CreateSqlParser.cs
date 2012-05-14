using System;
using System.Data.Common;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses create sql statements into a table storage request
    /// </summary>
    public class CreateSqlParser : ISqlParser
    {
        private readonly string[] commandTextParts;
        private readonly string tableName;

        public CreateSqlParser(string commandText)
        {
            commandTextParts = commandText.Split(new[] { ' ', '(', ',', ')', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            tableName = commandTextParts[2];
        }

        /// <summary>
        /// Gets the table storage request for the given settings and values
        /// </summary>
        public TableStorageRequest GetTableStorageRequest(TableStorageSettings settings, DbParameterCollection parameters)
        {
            var request = new TableStorageRequest(StorageHttpConstants.HttpMethod.Post, String.Format(settings.Uri.AbsoluteUri + "{0}", "Tables"), settings);
            request.Body.AddProperty("TableName", tableName);
            return request;
        }
    }
}