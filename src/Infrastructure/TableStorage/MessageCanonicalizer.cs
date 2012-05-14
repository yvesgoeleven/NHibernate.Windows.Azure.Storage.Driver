using System;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Canonicalize HTTP request messages.
    /// </summary>
    internal static class MessageCanonicalizer
    {
        /// <summary>
        /// An internal class that stores the canonicalized string version of an HTTP request.
        /// </summary>
        private class CanonicalizedString
        {
            private readonly StringBuilder canonicalizedString = new StringBuilder();

            /// <summary>
            /// Property for the canonicalized string.
            /// </summary>
            internal string Value
            {
                get
                {
                    return canonicalizedString.ToString();
                }
            }

            /// <summary>
            /// Constructor for the class.
            /// </summary>
            /// <param name="initialElement">The first canonicalized element to start the string with.</param>
            internal CanonicalizedString(string initialElement)
            {
                canonicalizedString.Append(initialElement);
            }

            /// <summary>
            /// Append additional canonicalized element to the string.
            /// </summary>
            /// <param name="element">An additional canonicalized element to append to the string.</param>
            internal void AppendCanonicalizedElement(string element)
            {
                canonicalizedString.Append(StorageHttpConstants.ConstChars.Linefeed);
                canonicalizedString.Append(element);
            }
        }

        /// <summary>
        /// Create a canonicalized string out of HTTP request header contents for signing 
        /// blob/queue requests with the Shared Authentication scheme. 
        /// </summary>
        /// <param name="address">The uri address of the HTTP request.</param>
        /// <param name="uriComponents">Components of the Uri extracted out of the request.</param>
        /// <param name="method">The method of the HTTP request (GET/PUT, etc.).</param>
        /// <param name="contentType">The content type of the HTTP request.</param>
        /// <param name="date">The date of the HTTP request.</param>
        /// <param name="headers">Should contain other headers of the HTTP request.</param>
        /// <returns>A canonicalized string of the HTTP request.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "Authentication algorithm requires canonicalization by converting to lower case")]
        internal static string CanonicalizeHttpRequest(
            Uri address,
            ResourceUriComponents uriComponents,
            string method,
            string contentType,
            string date,
            NameValueCollection headers)
        {
            // The first element should be the Method of the request.
            // I.e. GET, POST, PUT, or HEAD.
            var canonicalizedString = new CanonicalizedString(method);

            // The second element should be the MD5 value.
            // This is optional and may be empty.
            var httpContentMD5Value = string.Empty;

            // First extract all the content MD5 values from the header.
            var httpContentMD5Values = HttpRequestAccessor.GetHeaderValues(headers, StorageHttpConstants.HeaderNames.ContentMD5);

            // If we only have one, then set it to the value we want to append to the canonicalized string.
            if (httpContentMD5Values.Count == 1)
            {
                httpContentMD5Value = (string)httpContentMD5Values[0];
            }

            canonicalizedString.AppendCanonicalizedElement(httpContentMD5Value);

            // The third element should be the content type.
            canonicalizedString.AppendCanonicalizedElement(contentType);

            // The fourth element should be the request date.
            // See if there's an storage date header.
            // If there's one, then don't use the date header.
            var httpStorageDateValues = HttpRequestAccessor.GetHeaderValues(headers, StorageHttpConstants.HeaderNames.StorageDateTime);
            if (httpStorageDateValues.Count > 0)
            {
                date = null;
            }

            canonicalizedString.AppendCanonicalizedElement(date);

            // Look for header names that start with StorageHttpConstants.HeaderNames.PrefixForStorageHeader
            // Then sort them in case-insensitive manner.
            var httpStorageHeaderNameArray = new ArrayList();
            foreach (string key in headers.Keys)
            {
                if (key.ToLowerInvariant().StartsWith(StorageHttpConstants.HeaderNames.PrefixForStorageHeader, StringComparison.Ordinal))
                {
                    httpStorageHeaderNameArray.Add(key.ToLowerInvariant());
                }
            }

            httpStorageHeaderNameArray.Sort();

            // Now go through each header's values in the sorted order and append them to the canonicalized string.
            foreach (string key in httpStorageHeaderNameArray)
            {
                var canonicalizedElement = new StringBuilder(key);
                var delimiter = ":";
                var values = HttpRequestAccessor.GetHeaderValues(headers, key);

                // Go through values, unfold them, and then append them to the canonicalized element string.
                foreach (string value in values)
                {
                    // Unfolding is simply removal of CRLF.
                    var unfoldedValue = value.Replace(StorageHttpConstants.ConstChars.CarriageReturnLinefeed, string.Empty);

                    // Append it to the canonicalized element string.
                    canonicalizedElement.Append(delimiter);
                    canonicalizedElement.Append(unfoldedValue);
                    delimiter = ",";
                }

                // Now, add this canonicalized element to the canonicalized header string.
                canonicalizedString.AppendCanonicalizedElement(canonicalizedElement.ToString());
            }

            // Now we append the canonicalized resource element.
            var canonicalizedResource = GetCanonicalizedResource(address, uriComponents);
            canonicalizedString.AppendCanonicalizedElement(canonicalizedResource);

            return canonicalizedString.Value;
        }

        internal static string GetCanonicalizedResource(Uri address, ResourceUriComponents uriComponents)
        {
            // Algorithm is as follows
            // 1. Start with the empty string ("")
            // 2. Append the account name owning the resource preceded by a /. This is not 
            //    the name of the account making the request but the account that owns the 
            //    resource being accessed.
            // 3. Append the path part of the un-decoded HTTP Request-URI, up-to but not 
            //    including the query string.
            // 4. If the request addresses a particular component of a resource, like?comp=
            //    metadata then append the sub-resource including question mark (like ?comp=
            //    metadata)

            //KCA FIX THIS, THIS IS  NOT ACCORDING TO  THE RULES ABOVE!!!!!!!!!!!!!!!
            var canonicalizedResource = new StringBuilder(StorageHttpConstants.ConstChars.Slash);
            canonicalizedResource.Append(uriComponents.AccountName);

            // AbsolutePath starts with a '/'.
            canonicalizedResource.Append(address.AbsolutePath);

            var queryVariables = HttpUtility.ParseQueryString(address.Query);
            var compQueryParameterValue = queryVariables[StorageHttpConstants.QueryParams.QueryParamComp];
            if (compQueryParameterValue != null)
            {
                canonicalizedResource.Append(StorageHttpConstants.ConstChars.QuestionMark);
                canonicalizedResource.Append(StorageHttpConstants.QueryParams.QueryParamComp);
                canonicalizedResource.Append(StorageHttpConstants.QueryParams.SeparatorForParameterAndValue);
                canonicalizedResource.Append(compQueryParameterValue);
            }

            return canonicalizedResource.ToString();
        }


        /// <summary>
        /// Canonicalize HTTP header contents.
        /// </summary>
        /// <param name="request">An HttpWebRequest object.</param>
        /// <param name="uriComponents">Components of the Uri extracted out of the request.</param>
        /// <returns>The canonicalized string of the given HTTP request's header.</returns>
        internal static string CanonicalizeHttpRequest(HttpWebRequest request, ResourceUriComponents uriComponents)
        {
            return CanonicalizeHttpRequest(
                request.Address, uriComponents, request.Method, request.ContentType, string.Empty, request.Headers);
        }

        /// <summary>
        /// Creates a standard datetime string for the shared key lite authentication scheme.
        /// </summary>
        /// <param name="dateTime">DateTime value to convert to a string in the expected format.</param>
        /// <returns></returns>
        internal static string ConvertDateTimeToHttpString(DateTime dateTime)
        {
            // On the wire everything should be represented in UTC. This assert will catch invalid callers who
            // are violating this rule.
            Debug.Assert(dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue || dateTime.Kind == DateTimeKind.Utc);

            // 'R' means rfc1123 date which is what the storage services use for all dates...
            // It will be in the following format:
            // Sun, 28 Jan 2008 12:11:37 GMT
            return dateTime.ToString("R", CultureInfo.InvariantCulture);
        }

        private static void AppendStringToCanonicalizedString(StringBuilder canonicalizedString, string stringToAppend)
        {
            canonicalizedString.Append(StorageHttpConstants.ConstChars.Linefeed);
            canonicalizedString.Append(stringToAppend);
        }

        internal static string CanonicalizeHttpRequestForSharedKeyLite(HttpWebRequest request, ResourceUriComponents uriComponents, string date)
        {
            var canonicalizedString = new StringBuilder(date);
            AppendStringToCanonicalizedString(canonicalizedString, GetCanonicalizedResource(request.Address, uriComponents));

            return canonicalizedString.ToString();
        }
    }
}
