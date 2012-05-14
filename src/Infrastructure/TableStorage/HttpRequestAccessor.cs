using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Extracts various header values from Http requests.
    /// </summary>
    internal static class HttpRequestAccessor
    {
        /// <summary>
        /// A helper function for extracting HTTP header values from a NameValueCollection object.
        /// </summary>
        /// <param name="headers">A NameValueCollection object that should contain HTTP header name-values pairs.</param>
        /// <param name="headerName">Name of the header that we want to get values of.</param>
        /// <returns>A array list of values for the header. The values are in the same order as they are stored in the NameValueCollection object.</returns>
        internal static ArrayList GetHeaderValues(NameValueCollection headers, string headerName)
        {
            var arrayOfValues = new ArrayList();
            var values = headers.GetValues(headerName);

            if (values != null)
            {
                foreach (var value in values)
                {
                    // canonization formula requires the string to be left trimmed.
                    arrayOfValues.Add(value.TrimStart());
                }
            }

            return arrayOfValues;
        }


        /// <summary>
        /// Constructs an URI given all its constituents
        /// </summary>
        /// <param name="endpoint">
        /// This is the service endpoint in case of path-style URIs and a host suffix in case of host-style URIs
        /// IMPORTANT: This does NOT include the service name or account name
        /// </param>
        /// <param name="uriComponents">Uri constituents</param>
        /// <param name="pathStyleUri">Indicates whether to construct a path-style Uri (true) or host-style URI (false)</param>
        /// <returns>Full uri</returns>
        public static Uri ConstructResourceUri(Uri endpoint, ResourceUriComponents uriComponents, bool pathStyleUri)
        {
            return pathStyleUri ?
                    ConstructPathStyleResourceUri(endpoint, uriComponents) :
                    ConstructHostStyleResourceUri(endpoint, uriComponents);
        }

        /// <summary>
        /// Constructs a path-style resource URI given all its constituents
        /// </summary>
        private static Uri ConstructPathStyleResourceUri(Uri endpoint, ResourceUriComponents uriComponents)
        {
            var path = new StringBuilder(string.Empty);
            if (uriComponents.AccountName != null)
            {
                path.Append(uriComponents.AccountName);

                if (uriComponents.ContainerName != null)
                {
                    path.Append(StorageHttpConstants.ConstChars.Slash);
                    path.Append(uriComponents.ContainerName);

                    if (uriComponents.RemainingPart != null)
                    {
                        path.Append(StorageHttpConstants.ConstChars.Slash);
                        path.Append(uriComponents.RemainingPart);
                    }
                }
            }

            return ConstructUriFromUriAndString(endpoint, path.ToString());
        }

        /// <summary>
        /// Constructs a host-style resource URI given all its constituents
        /// </summary>
        private static Uri ConstructHostStyleResourceUri(Uri hostSuffix, ResourceUriComponents uriComponents)
        {
            if (uriComponents.AccountName == null)
            {
                // When there is no account name, full URI is same as hostSuffix
                return hostSuffix;
            }
            // accountUri will be something like "http://accountname.hostSuffix/" and then we append
            // container name and remaining part if they are present.
            var accountUri = ConstructHostStyleAccountUri(hostSuffix, uriComponents.AccountName);
            var path = new StringBuilder(string.Empty);
            if (uriComponents.ContainerName != null)
            {
                path.Append(uriComponents.ContainerName);

                if (uriComponents.RemainingPart != null)
                {
                    path.Append(StorageHttpConstants.ConstChars.Slash);
                    path.Append(uriComponents.RemainingPart);
                }
            }

            return ConstructUriFromUriAndString(accountUri, path.ToString());
        }


        /// <summary>
        /// Given the host suffix part, service name and account name, this method constructs the account Uri
        /// </summary>
        private static Uri ConstructHostStyleAccountUri(Uri hostSuffix, string accountName)
        {
            // Example: 
            // Input: serviceEndpoint="http://blob.windows.net/", accountName="youraccount"
            // Output: accountUri="http://youraccount.blob.windows.net/"
            var serviceUri = hostSuffix;

            // serviceUri in our example would be "http://blob.windows.net/"
            var accountUriString = string.Format(CultureInfo.InvariantCulture,
                                        "{0}{1}{2}.{3}:{4}/",
                                        serviceUri.Scheme,
                                        Uri.SchemeDelimiter,
                                        accountName,
                                        serviceUri.Host,
                                        serviceUri.Port);

            return new Uri(accountUriString);
        }

        private static Uri ConstructUriFromUriAndString(
            Uri endpoint,
            string path)
        {
            // This is where we encode the url path to be valid
            var encodedPath = HttpUtility.UrlPathEncode(path);
            return new Uri(endpoint, encodedPath);
        }
    }
}
