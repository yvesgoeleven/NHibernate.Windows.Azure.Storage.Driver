using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;

namespace NHibernate.Drivers.Azure.TableStorage
{
	/// <summary>
    /// </summary>
    /// <remarks>
	/// If we inherit from IDbConnection, we get an exception later on, when this is casted to a DbConnection! 
    /// From DbConnection to IDbConnetion isn't a problem, even though DbConnection doens't implement IDbConnection
    /// </remarks>
	public class TableStorageConnection : DbConnection
	{
	    private readonly TableStorageSettings tableStorageSettings;
        
        /// <summary>
        /// Creates a new instance of TableStorageConnection based on the settings passed
        /// </summary>
        /// <param name="settings"></param>
        public TableStorageConnection(TableStorageSettings settings)
        {
            tableStorageSettings = settings;
            Tracker = new ETagTracker();
        }

        public ETagTracker Tracker { get; set; }

	    /// <summary>
	    /// Starts a database transaction.
	    /// </summary>
	    /// <returns>
	    /// An object representing the new transaction.
	    /// </returns>
	    /// <param name="isolationLevel">Specifies the isolation level for the transaction.
	    ///                 </param>
	    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
	    {
	        return new TableStorageTransaction(this, isolationLevel);
	    }

	    /// <summary>
	    /// Changes the current database for an open connection.
	    /// </summary>
	    /// <param name="databaseName">Specifies the name of the database for the connection to use.
	    ///                 </param><filterpriority>2</filterpriority>
	    public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

	    /// <summary>
	    /// Closes the connection to the database. This is the preferred method of closing any open connection.
	    /// </summary>
	    /// <exception cref="T:System.Data.Common.DbException">The connection-level error that occurred while opening the connection. 
	    ///                 </exception><filterpriority>1</filterpriority>
	    public override void Close()
		{
			throw new NotImplementedException();
		}

	    /// <summary>
	    /// Gets or sets the string used to open the connection.
	    /// </summary>
	    /// <returns>
	    /// The connection string used to establish the initial connection. The exact contents of the connection string depend on the specific data source for this connection. The default value is an empty string.
	    /// </returns>
	    /// <filterpriority>1</filterpriority>
	    public override string ConnectionString
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

	    /// <summary>
	    /// Creates and returns a <see cref="T:System.Data.Common.DbCommand"/> object associated with the current connection.
	    /// </summary>
	    /// <returns>
	    /// A <see cref="T:System.Data.Common.DbCommand"/> object.
	    /// </returns>
	    protected override DbCommand CreateDbCommand()
		{
			return new TableStorageCommand ( tableStorageSettings ) { Connection = this };
		}

	    /// <summary>
	    /// Gets the name of the database server to which to connect.
	    /// </summary>
	    /// <returns>
	    /// The name of the database server to which to connect. The default value is an empty string.
	    /// </returns>
	    /// <filterpriority>1</filterpriority>
	    public override string DataSource
		{
			get { throw new NotImplementedException(); }
		}

	    /// <summary>
	    /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the connection is opened.
	    /// </summary>
	    /// <returns>
	    /// The name of the current database or the name of the database to be used after a connection is opened. The default value is an empty string.
	    /// </returns>
	    /// <filterpriority>1</filterpriority>
	    public override string Database
		{
			get { throw new NotImplementedException(); }
		}

	    /// <summary>
	    /// Opens a database connection with the settings specified by the <see cref="P:System.Data.Common.DbConnection.ConnectionString"/>.
	    /// </summary>
	    /// <filterpriority>1</filterpriority>
	    public override void Open()
		{
			throw new NotImplementedException();
		}

	    /// <summary>
	    /// Gets a string that represents the version of the server to which the object is connected.
	    /// </summary>
	    /// <returns>
	    /// The version of the database. The format of the string returned depends on the specific type of connection you are using.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    public override string ServerVersion
		{
			get { throw new NotImplementedException(); }
		}

	    /// <summary>
	    /// Gets a string that describes the state of the connection.
	    /// </summary>
	    /// <returns>
	    /// The state of the connection. The format of the string returned depends on the specific type of connection you are using.
	    /// </returns>
	    /// <filterpriority>1</filterpriority>
	    public override ConnectionState State
		{
			get { throw new NotImplementedException(); }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="restrictionValues"></param>
        /// <returns></returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            return new DataTable(collectionName);
        }

    }

    public class ETagTracker
    {
        private Dictionary<string, string> etags = new Dictionary<string, string>();

        public string GetIfMatchHeaderFor(string tableName, string rowkey, string partitionKey)
        {
            var header = "*";
            etags.TryGetValue(tableName + "_" + rowkey + "_" + partitionKey, out header);
            return header;
        }

        public void TrackEtagFor(string etag, string tableName, string rowkey, string partitionKey)
        {
            etags[tableName + "_" + rowkey + "_" + partitionKey] = etag;
        }
    }
}