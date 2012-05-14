using System.Data.Common;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Parses sql statements into table storage requests
    /// </summary>
    public interface ISqlParser
    {
        /// <summary>
        /// Gets the table storage request for the given settings and values
        /// </summary>
        TableStorageRequest GetTableStorageRequest(TableStorageSettings settings, DbParameterCollection parameters);
    }
}