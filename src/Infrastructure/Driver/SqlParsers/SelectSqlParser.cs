using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses select sql queries into a table storage request
    /// </summary>
    public class SelectSqlParser : ISqlParser
    {
        private readonly string filter;
        private readonly string tableName;
       
        /// <summary>
        /// Creates a new instance of sql parser
        /// </summary>
        /// <param name="commandText"></param>
        public SelectSqlParser(string commandText)
        {
            commandText = RemoveTableNameBracketsFromCommandText(commandText);
            tableName = ExtractTableName(commandText);
            if (!commandText.Contains("WHERE") && !commandText.Contains("where")) return;
            filter = CreateFilterFromWhereStatement(commandText);
        }
        
        /// <summary>
        /// Gets the table storage request for the given settings and values
        /// </summary>
        public TableStorageRequest GetTableStorageRequest(TableStorageSettings settings, DbParameterCollection dbParameterCollection)
        {
            var requestUri = GetRequestUri(settings.Uri.AbsoluteUri, dbParameterCollection);
            return new TableStorageRequest(StorageHttpConstants.HttpMethod.Get, requestUri, settings);
        }

        private static string RemoveTableNameBracketsFromCommandText(string commandText)
        {
            return commandText.Replace("[", "").Replace("]", "");
        }

        private static String ExtractTableName(string commandText)
        {
            return new Regex(@"\sFROM\s(\w)*", RegexOptions.IgnoreCase).Match(commandText).Value.Remove(0, 6);
        }

        private static string CreateFilterFromWhereStatement(string commandText)
        {
            var nameValuePairs = new Regex(@"(\w*\d*)_(\.\w*\s*)(\=)\s*[\w\d\?]*").Matches(commandText);
            var whereStatement = new Regex(@"WHERE\s(.)*(ORDER BY)?", RegexOptions.IgnoreCase).Match(commandText).Value.Remove(0, 6).Replace("ORDER BY", "").Replace("order by", "");
            var parsedNameValuePairs = Parse(nameValuePairs);
            var f = "?$filter=" + Replace(nameValuePairs, parsedNameValuePairs, whereStatement);
            f = SingularizeWhiteSpaces(f);
            return f;
        }

        private static string SingularizeWhiteSpaces(string filter)
        {
            return Regex.Replace(filter, @"[\s]+", " ", RegexOptions.IgnoreCase);
        }

        private string GetRequestUri(string baseUri, DbParameterCollection parameters)
        {
            var requestUri = String.Format(baseUri + "{0}()", tableName);
            if (filter != null)
            {
                var values = ExtractValuesFrom(parameters);
                requestUri += String.Format(filter, values.ToArray());
            }
            return requestUri;
        }

        private static IEnumerable<object> ExtractValuesFrom(DbParameterCollection parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                if (parameter.DbType == DbType.Boolean)
                {
                    yield return parameter.Value.ToString().ToLower();
                }
                else if (parameter.DbType == DbType.Int16
                    || parameter.DbType == DbType.Int32)
                {
                    yield return parameter.Value;
                }
                else if (parameter.DbType == DbType.Int64)
                {
                    yield return parameter.Value + "L";
                }
                else if (parameter.DbType == DbType.Guid)
                {
                    yield return "guid'" + parameter.Value + "'";
                }
                else if (parameter.DbType == DbType.DateTime)
                {
                    yield return "datetime'" + parameter.Value + "'";
                }
                else
                {
                    yield return "'" + parameter.Value + "'";
                }
            }
        }

        private static string Replace(MatchCollection nameValuePairs, IList<string> parsedNameValuePairs, string statement)
        {
            for (var i = 0; i < nameValuePairs.Count; i++)
            {
                statement = statement.Replace(nameValuePairs[i].Value, parsedNameValuePairs[i]);
            }
            return statement;
        }

        private static IList<string> Parse(MatchCollection nameValuePairs)
        {
            var occurance = 0;
            return (from Match pair in nameValuePairs select Parse(pair.Value, occurance++)).ToList();
        }

        private static string Parse(string part, int occurance)
        {
            var parsed = part;

            if (part.Contains("_.")) parsed = " " + parsed.Substring(part.IndexOf("_.") + 2) + " ";
            if (part.Contains("=")) parsed = parsed.Replace("=", " eq ");
            if (part.Contains("?")) parsed = parsed.Replace("?", " {" + occurance + "} ");
            if (part.Contains("p" + occurance)) parsed = parsed.Replace("p" + occurance , " {" + occurance + "} ");

            return "(" + parsed.Trim() + ")";
        }

    }
}