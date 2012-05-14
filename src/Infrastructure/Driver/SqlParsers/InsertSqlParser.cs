using System;
using System.Data.Common;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses insert sql statements into a table storage request
    /// </summary>
    public class InsertSqlParser : ISqlParser
    {
        private readonly string[] commandTextParts;
        private readonly string tableName;

        public InsertSqlParser(string commandText)
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
            Array.Copy(commandTextParts, 3, parameterNames, 0, parameters.Count);
            var request = new TableStorageRequest(StorageHttpConstants.HttpMethod.Post, String.Format(settings.Uri.AbsoluteUri + "{0}", tableName), settings);
            for (var i = 0; i < parameterNames.Length; i++)
            {
                request.Body.AddProperty(parameterNames[i], parameters[i].Value, parameters[i].DbType);
            }
            return request;
        }
    }
}