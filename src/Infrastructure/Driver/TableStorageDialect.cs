using System;
using NHibernate.Dialect;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// The dialect that indicates what sql superset is supported by table storage
    /// </summary>
    public class TableStorageDialect : MsSql2005Dialect
    {
        /// <summary>
        /// Gets the max alias length
        /// </summary>
        /// <remarks>
        /// The only way I've found to go from a column alias to the actual column name
        /// is by stripping the information after the first underscore '_'.
        /// If the name is to long, it is truncated -> set MaxAliasLength high enough...
        /// Note: this currently doesn't allow you to use underscores in your column names
        /// </remarks>
        public override int MaxAliasLength
        {
            get { return Int32.MaxValue; }
        }

        public override string GetTypeName(SqlTypes.SqlType sqlType)
        {
            switch (sqlType.DbType)
            { 
                case System.Data.DbType.Binary:
                case System.Data.DbType.Boolean:
                case System.Data.DbType.DateTime:
                case System.Data.DbType.Double:
                case System.Data.DbType.Guid:
                case System.Data.DbType.Int32:
                case System.Data.DbType.Int64:
                case System.Data.DbType.String:

                    return base.GetTypeName(sqlType);
            }

            throw new NotSupportedException("Azure table storage does not support the type: " + sqlType.DbType.ToString());
        }
    }
}
