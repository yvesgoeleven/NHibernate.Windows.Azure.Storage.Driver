using System;
using System.Linq;
using System.Net;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Represents a response from the table storage services
    /// </summary>
    public class TableStorageResponse
    {
        public TableStorageResponse(HttpStatusCode statusCode, string response, WebHeaderCollection headers)
        {
            StatusCode = statusCode;

            Body = String.IsNullOrEmpty(response) ? null : new TableStorageResponseBody(response);

            var values = headers.GetValues(StorageHttpConstants.HeaderNames.PrefixForTableContinuation + StorageHttpConstants.HeaderNames.NextRowKey);
            NextRowKey = values != null ? values.FirstOrDefault() : null;

            values = headers.GetValues(StorageHttpConstants.HeaderNames.PrefixForTableContinuation + StorageHttpConstants.HeaderNames.NextPartitionKey);
            NextPartitionKey = values != null ? values.FirstOrDefault() : null;

        }

        /// <summary>
        /// Gets the status code of the response
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Gets the body of the response
        /// </summary>
        public TableStorageResponseBody Body { get; internal set; }

        public string NextRowKey { get; set; }

        public string NextPartitionKey { get; set; }
    }
}