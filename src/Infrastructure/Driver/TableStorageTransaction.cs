using System.Data;
using System.Data.Common;

namespace NHibernate.Drivers.Azure.TableStorage
{
    public class TableStorageTransaction : DbTransaction
    {
        private readonly DbConnection dbConnection;
        private readonly IsolationLevel isolationLevel;

        public TableStorageTransaction(DbConnection dbConnection, IsolationLevel isolationLevel)
        {
            this.dbConnection = dbConnection;
            this.isolationLevel = isolationLevel;
        }

        public override void Commit()
        {
           
        }

        public override void Rollback()
        {
            
        }

        protected override DbConnection DbConnection
        {
            get { return dbConnection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return isolationLevel; }
        }
    }
}
