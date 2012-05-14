using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// A DbCommand implementation that converts the command text into a tablestorage request and converts the response into a datareader
    /// </summary>
    public class TableStorageCommand : DbCommand
    {
        private readonly TableStorageSettings settings;
        private DbParameterCollection dbParameterCollection;

        /// <summary>
        /// Creates a new instance of TableStorageCommand using the passed in settings
        /// </summary>
        /// <param name="tableStorageSettings"></param>
        public TableStorageCommand(TableStorageSettings tableStorageSettings)
        {
            settings = tableStorageSettings;
        }

        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        /// <returns>
        /// The text command to execute. The default value is an empty string ("").
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <returns>
        /// The time in seconds to wait for the command to execute.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int CommandTimeout { get; set; }

        /// <summary>
        /// Indicates or specifies how the <see cref="P:System.Data.Common.DbCommand.CommandText"/> property is interpreted.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Data.CommandType"/> values. The default is Text.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.Common.DbConnection"/> used by this <see cref="T:System.Data.Common.DbCommand"/>.
        /// </summary>
        /// <returns>
        /// The connection to the data source.
        /// </returns>
        protected override DbConnection DbConnection { get; set; }

        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"/> when used by the Update method of a <see cref="T:System.Data.Common.DbDataAdapter"/>.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Data.UpdateRowSource"/> values. The default is Both unless the command is automatically generated. Then the default is None.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="P:System.Data.Common.DbCommand.DbTransaction"/> within which this <see cref="T:System.Data.Common.DbCommand"/> object executes.
        /// </summary>
        /// <returns>
        /// The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).
        /// </returns>
        protected override DbTransaction DbTransaction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a customized interface control.
        /// </summary>
        /// <returns>
        /// true, if the command object should be visible in a control; otherwise false. The default is true.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override bool DesignTimeVisible { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="T:System.Data.Common.DbParameter"/> objects.
        /// </summary>
        /// <returns>
        /// The parameters of the SQL statement or stored procedure.
        /// </returns>
        protected override DbParameterCollection DbParameterCollection
        {
            get { return (dbParameterCollection = dbParameterCollection ?? new TableStorageParameterCollection()); }
        }

        /// <summary>
        /// Attempts to cancels the execution of a <see cref="T:System.Data.Common.DbCommand"/>.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public override void Cancel(){}

        /// <summary>
        /// Creates a new instance of a <see cref="T:System.Data.Common.DbParameter"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbParameter"/> object.
        /// </returns>
        protected override DbParameter CreateDbParameter()
        {
            return new TableStorageParameter();
        }

        /// <summary>
        /// Executes the command text against the connection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbDataReader"/>.
        /// </returns>
        /// <param name="behavior">An instance of <see cref="T:System.Data.CommandBehavior"/>.
        ///                 </param>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            var request = new SelectSqlParser(CommandText).GetTableStorageRequest(settings, DbParameterCollection);
            return new TableStorageDataReader(request, ((TableStorageConnection)Connection).Tracker);
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override int ExecuteNonQuery()
        {
            ISqlParser parser = null;
            switch (CommandMethod.ToUpper())
            {
                case "INSERT":
                    parser = new InsertSqlParser(CommandText);
                    break;
                case "UPDATE":
                    parser = new UpdateSqlParser(CommandText, ((TableStorageConnection)Connection).Tracker);
                    break;
                case "DELETE":
                    parser = new DeleteSqlParser(CommandText);
                    break;
                case "CREATE":
                    parser = new CreateSqlParser(CommandText);
                    break;
            }

            if( parser != null )
            {
                var request = parser.GetTableStorageRequest(settings, Parameters);
                request.Send();
            }

            return 1;
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public override void Prepare()
        {
            throw new NotImplementedException();
        }
        
        private string CommandMethod
        {
            get { return CommandText.Substring(0, CommandText.IndexOf(' ')); }
        }
  

    }
}