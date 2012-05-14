using System;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Settings for creating the table storage requests
    /// </summary>
    public class TableStorageSettings
    {
        /// <summary>
        /// Creates a new instance of TableStorageSettings
        /// </summary>
        /// <param name="uri">The uri where you table storage services are located.</param>
        /// <param name="accountName">The account name used to connect to the table storage services.</param>
        /// <param name="sharedKey">The shared key of the accountused to connect to the table storage services.</param>
        public TableStorageSettings(Uri uri, string accountName, string sharedKey)
        {
            Uri = uri;
            AccountName = accountName;
            SharedKey = sharedKey;
        }

        /// <summary>
        /// Gets the uri where you table storage services are located
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the account name used to connect to the table storage services.
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// Gets the shared key of the accountused to connect to the table storage services.
        /// </summary>
        public string SharedKey { get; private set; }

        /// <summary>
        /// Gets or sets the original connection string
        /// </summary>
        public string ConnectionString { get; internal set; }

    }
}
