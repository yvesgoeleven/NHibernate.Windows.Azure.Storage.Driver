using System;
using System.Net;
using System.Text;
using System.IO;
using log4net;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Represents a request to the table storage services
    /// </summary>
    public class TableStorageRequest
    {
        private readonly TableStorageSettings settings;
        private TableStorageRequestBody body;

        /// <summary>
        /// Creates a new instance of TableStorageRequest
        /// </summary>
        public TableStorageRequest(string method, string uri, TableStorageSettings settings)
            : this(method, new Uri(uri), settings) { }

        /// <summary>
        /// Creates a new instance of TableStorageRequest
        /// </summary>
        public TableStorageRequest(string method, Uri uri, TableStorageSettings settings)
        {
            Method = method;
            Uri = uri;
            this.settings = settings;
        }

        /// <summary>
        /// Gets the method used to send the request
        /// </summary>
        public string Method { get; internal set; }

        /// <summary>
        /// Gets the uri used to send the request to
        /// </summary>
        public Uri Uri { get; internal set; }

        /// <summary>
        /// Gets the ifmatch header
        /// </summary>
        public string IfMatch { get; set; }

        /// <summary>
        /// Gets the body
        /// </summary>
        public TableStorageRequestBody Body { 
            get {
                if(Method.Equals(StorageHttpConstants.HttpMethod.Get, StringComparison.InvariantCultureIgnoreCase)
                    || Method.Equals(StorageHttpConstants.HttpMethod.Delete, StringComparison.InvariantCultureIgnoreCase)){
                    throw new InvalidOperationException(String.Format("There is no body for the '{0}' method",  Method));
                }
                return body = body ?? new TableStorageRequestBody();
            }
        }

        /// <summary>
        /// Gets or sets the continuation token 
        /// </summary>
        public string NextRowKey { get; set; }

        /// <summary>
        /// Gets or sets the continuation token 
        /// </summary>
        public string NextPartitionKey { get; set; }

        /// <summary>
        /// Sends the request and wait for the response
        /// </summary>
        public TableStorageResponse Send()
        {
            var credentials = new SharedKeyCredentials(settings.AccountName, Convert.FromBase64String(settings.SharedKey));

            var adaptedUri = Uri.ToString();

            //Add continuation tokes if any
            if (NextRowKey != null)
            {
                var separator = adaptedUri.Contains("?") ? "&" : "?";
                adaptedUri = adaptedUri + separator + StorageHttpConstants.HeaderNames.NextRowKey + "=" + NextRowKey;
            }
            if (NextPartitionKey != null) 
            {
                var separator = adaptedUri.Contains("?") ? "&" : "?";
                adaptedUri =  adaptedUri + separator + StorageHttpConstants.HeaderNames.NextPartitionKey + "=" + NextPartitionKey;
            }
                
            //Build request 
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(adaptedUri));
            httpWebRequest.ContentType = "application/atom+xml";
            httpWebRequest.Method = Method;
            var requestData = body == null ? new byte[0] : Encoding.UTF8.GetBytes(body.ToString());
            httpWebRequest.ContentLength = requestData.Length;

            //Sign the request after setting the content length
            credentials.SignRequestForSharedKeyLite(httpWebRequest, new ResourceUriComponents(settings.AccountName));
            if (!String.IsNullOrEmpty(IfMatch))
            {
                httpWebRequest.Headers.Add(StorageHttpConstants.HeaderNames.IfMatch, IfMatch);
            }

          
            try
            {
                if (null != body)
                {
                    //Set request body 
                    using (var requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(requestData, 0, requestData.Length);
                    }
                }

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            var responseContent = responseReader.ReadToEnd();
                            return new TableStorageResponse(response.StatusCode, responseContent, response.Headers);
                        }

                    }
                }
            }
            catch (WebException ex)
            {
                Log(ex);
            }
            return null;
        }

        private void Log(WebException ex)
        {
            var logger = LogManager.GetLogger(typeof(TableStorageRequest));
            if(logger != null) logger.Error(ex.ToString());
        }
    }
}