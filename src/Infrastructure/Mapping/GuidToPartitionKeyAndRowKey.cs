using System;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NHibernate.Drivers.Azure.TableStorage.Mapping
{
    public class GuidToPartitionKeyAndRowKey : IUserType
    {
        bool IUserType.Equals(object x, object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            return new Guid((string) rs[names[0]]);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            ((IDataParameter) cmd.Parameters[index]).Value = value;
            ((IDataParameter) cmd.Parameters[index + 1]).Value = value;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes
        {
            get
            {
                var types = new SqlType[2];
                types[0] = new SqlType(DbType.String);
                types[1] = new SqlType(DbType.String);
                return types;
            }
        }

        public System.Type ReturnedType
        {
            get { return typeof(Guid); }
        }

        public bool IsMutable
        {
            get { return false; }
        }
    }
}