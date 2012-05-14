using System;
using System.Data;
using NHibernate.Driver;
using NHibernate.SqlCommand;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// A driver for azure table storage
    /// </summary>
    public class TableStorageDriver : DriverBase
    {
        private readonly TableStorageSettings tableStorageSettings;

        /// <summary>
        /// Create a new instance of TableStorageDriver
        /// </summary>
        public TableStorageDriver(TableStorageSettings settings)
        {
            tableStorageSettings = settings;
        }

        /// <summary>
        /// Create a new connection
        /// </summary>
        public override System.Data.IDbConnection CreateConnection()
        {
            return new TableStorageConnection(tableStorageSettings);
        }

        /// <summary>
        /// Create a new command
        /// </summary>
        public override System.Data.IDbCommand CreateCommand()
        {
            return CreateConnection().CreateCommand();
        }

        /// <summary>
        /// Gets a value indicating wheter named prefixes in sql are to be used
        /// </summary>
        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating wheter named prefixes in parameters are to be used
        /// </summary>
        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the named prefix to be used
        /// </summary>
        public override string NamedPrefix
        {
            get { return String.Empty; }
        }

        
    }
}