using System.Collections.Generic;
using System.Data;
using NHibernate.Connection;
using NHibernate.Driver;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// A provider for table storage connections
    /// </summary>
	public class TableStorageConnectionProvider : IConnectionProvider
	{
        private TableStorageDriver driver;

        /// <summary>
        /// Configure the provider
        /// </summary>
        /// <param name="settings"></param>
		public void Configure(IDictionary<string, string> settings)
		{
		    var tableStorageSettings = new ConnectionStringParser().Parse(settings["connection.connection_string"]);
            driver = new TableStorageDriver(tableStorageSettings);
		}

        /// <summary>
        /// Close the connection
        /// </summary>
        /// <param name="conn"></param>
		public void CloseConnection(IDbConnection conn) {}

        /// <summary>
        /// Gets the driver
        /// </summary>
		public IDriver Driver { get { return driver; } }

        /// <summary>
        /// Gets the connection
        /// </summary>
        /// <returns></returns>
		public IDbConnection GetConnection()
		{
            return driver.CreateConnection();
		}

        /// <summary>
        /// Dispose the current instance
        /// </summary>
		public void Dispose() {}
	}
}