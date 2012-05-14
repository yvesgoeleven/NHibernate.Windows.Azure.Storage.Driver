using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Net;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Objects of this class contain the credentials (name and key) of a storage account.
    /// </summary>
    public class SharedKeyCredentials
    {
        private readonly string accountName;
        private readonly byte[] key;

        /// <summary>
        /// Create a SharedKeyCredentials object given an account name and a shared key.
        /// </summary>
        public SharedKeyCredentials(string accountName, byte[] key)
        {
            this.accountName = accountName;
            this.key = key;
        }

        /// <summary>
        /// Signs the request appropriately to make it an authenticated request.
        /// This method takes the URI components as decoding the URI components requires the knowledge
        /// of whether the URI is in path-style or host-style and a host-suffix if it's host-style.
        /// </summary>
        public void SignRequest(HttpWebRequest request, ResourceUriComponents uriComponents)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            var message = MessageCanonicalizer.CanonicalizeHttpRequest(request, uriComponents);
            var computedBase64Signature = ComputeMacSha(message);
            request.Headers.Add(StorageHttpConstants.HeaderNames.Authorization,
                                string.Format(CultureInfo.InvariantCulture,
                                              "{0} {1}:{2}",
                                              StorageHttpConstants.AuthenticationSchemeNames.SharedKeyAuthSchemeName,
                                              accountName,
                                              computedBase64Signature));
        }

        /// <summary>
        /// Signs requests using the SharedKeyLite authentication scheme with is used for the table storage service.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lite",
                                                          Justification = "Name of the authentication scheme in the REST protocol")]
        public void SignRequestForSharedKeyLite(HttpWebRequest request, ResourceUriComponents uriComponents)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // add the date header to the request
            var dateString = MessageCanonicalizer.ConvertDateTimeToHttpString(DateTime.UtcNow);
            request.Headers.Add(StorageHttpConstants.HeaderNames.StorageDateTime, dateString);

            // compute the signature and add the authentication scheme
            var message = MessageCanonicalizer.CanonicalizeHttpRequestForSharedKeyLite(request, uriComponents, dateString);
            var computedBase64Signature = ComputeMacSha(message);
            request.Headers.Add(StorageHttpConstants.HeaderNames.Authorization,
                                string.Format(CultureInfo.InvariantCulture,
                                              "{0} {1}:{2}",
                                              StorageHttpConstants.AuthenticationSchemeNames.SharedKeyLiteAuthSchemeName,
                                              accountName,
                                              computedBase64Signature));
        }

        private string ComputeMacSha(string canonicalizedString)
        {
            var dataToMAC = System.Text.Encoding.UTF8.GetBytes(canonicalizedString);

            using (var hmacsha1 = new HMACSHA256(key))
            {
                return System.Convert.ToBase64String(hmacsha1.ComputeHash(dataToMAC));
            }
        }
    }
}
