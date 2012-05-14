using System;
using System.Collections.Generic;
using System.Configuration;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses the connection string
    /// </summary>
    public class ConnectionStringParser
    {
        /// <summary>
        /// Parse a connection string into a table storage settings object
        /// </summary>
        public TableStorageSettings Parse(string connectionString)
        {
            string uri = null; 
            string protocol = null; 
            string accountName = null;
            string sharedKey = null;

            var nameValuePairs = connectionString.Split(';');

            uri = connectionString.Contains("UseDevelopmentStorage=true") ? 
                UseDevelopmentStorageSettings(nameValuePairs, out accountName, out sharedKey) : 
                UseOnlineSettings(nameValuePairs, uri, ref protocol, ref accountName, ref sharedKey);
            
            if (uri == null && protocol != null && accountName != null)
                uri = protocol + "://" + accountName + "." + StorageHttpConstants.StandardPortalEndpoints.TableStorage + StorageHttpConstants.StandardPortalEndpoints.StorageHostSuffix + "/";

            if (uri == null) throw new ConfigurationErrorsException("Bad connection string format, missing key 'uri'");
            if (sharedKey == null) throw new ConfigurationErrorsException("Bad connection string format, missing key 'shared_key'");
            if (accountName == null) throw new ConfigurationErrorsException("Bad connection string format, missing key 'account_name'");
            
            return new TableStorageSettings(new Uri(uri), accountName, sharedKey) { ConnectionString = connectionString};
            
        }

        /// <summary>
        /// Use the online setting information in the connection string
        /// </summary>
        private static string UseOnlineSettings(IEnumerable<string> nameValuePairs, string uri, ref string protocol, ref string accountName, ref string sharedKey)
        {
            foreach (var pair in nameValuePairs)
            {
                var equalsIndex = pair.IndexOf("=");
                var key = pair.Substring(0, equalsIndex).Trim();
                var value = pair.Substring(equalsIndex + 1);
                switch (key)
                {
                    case "uri":
                        uri = value;
                        break;
                    case "DefaultEndpointsProtocol":
                        protocol = value;
                        break;
                    case "AccountKey":
                    case "shared_key":
                        sharedKey = value;
                        break;
                    case "AccountName":
                    case "account_name":
                        accountName = value;
                        break;
                    default:
                        throw new ConfigurationErrorsException(
                            string.Format("Bad connection string format, unrecognized key '{0}'", key));
                }
            }
            return uri;
        }

        /// <summary>
        /// Use the development storage settings
        /// </summary>
        private static string UseDevelopmentStorageSettings(IEnumerable<string> nameValuePairs, out string accountName, out string sharedKey)
        {
            var uri = "http://127.0.0.1:10002/devstoreaccount1/";

            foreach (var pair in nameValuePairs)
            {
                var equalsIndex = pair.IndexOf("=");
                var key = pair.Substring(0, equalsIndex).Trim();
                var value = pair.Substring(equalsIndex + 1);
                switch (key)
                { 
                    case "DevelopmentStorageProxyUri":
                        uri = value + ":10002/devstoreaccount1/";
                        break;
                }
            }

            accountName = "devstoreaccount1";
            sharedKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
            return uri;
        }
    }
}
