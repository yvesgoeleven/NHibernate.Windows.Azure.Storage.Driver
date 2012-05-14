using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// A data reader that provides access to the entries in a table storage response body 
    /// </summary>
    public class TableStorageDataReader : DbDataReader
    {
        private readonly TableStorageRequest request;
        private TableStorageResponse response;
        private XDocument data;
        private IEnumerator<XElement> entryElementsEnumerator;
        private XElement propertiesElement;
        private readonly Regex aliasPattern = new Regex("[[0-9]+_]*");

        /// <summary>
        /// Create a new instance of TableStorageDataReader
        /// </summary>
        /// <param name="request"></param>
        public TableStorageDataReader(TableStorageRequest request, ETagTracker tracker)
        {
            this.request = request;
            this.Tracker = tracker;
        }

        /// <summary>
        /// Closes the <see cref="T:System.Data.Common.DbDataReader"/> object.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public override void Close()
        {
            data = null;
            entryElementsEnumerator = null;
            propertiesElement = null;
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <returns>
        /// The depth of nesting for the current row.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override int Depth
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <returns>
        /// The number of columns in the current row.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override int FieldCount
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column, starting at location indicated by <paramref name="dataOffset"/>, into the buffer, starting at the location indicated by <paramref name="bufferOffset"/>.
        /// </summary>
        /// <returns>
        /// The actual number of bytes read.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><param name="dataOffset">The index within the row from which to begin the read operation.
        ///                 </param><param name="buffer">The buffer into which to copy the data.
        ///                 </param><param name="bufferOffset">The index with the buffer to which the data will be copied.
        ///                 </param><param name="length">The maximum number of characters to read.
        ///                 </param><filterpriority>1</filterpriority>
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a single character.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a stream of characters from the specified column, starting at location indicated by <paramref name="dataOffset"/>, into the buffer, starting at the location indicated by <paramref name="bufferOffset"/>.
        /// </summary>
        /// <returns>
        /// The actual number of characters read.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><param name="dataOffset">The index within the row from which to begin the read operation.
        ///                 </param><param name="buffer">The buffer into which to copy the data.
        ///                 </param><param name="bufferOffset">The index with the buffer to which the data will be copied.
        ///                 </param><param name="length">The maximum number of characters to read.
        ///                 </param><filterpriority>1</filterpriority>
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets name of the data type of the specified column.
        /// </summary>
        /// <returns>
        /// A string representing the name of the data type.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="T:System.DateTime"/> object.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="T:System.Decimal"/> object.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a double-precision floating point number.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an <see cref="T:System.Collections.IEnumerator"/> that can be used to iterate through the rows in the data reader.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> that can be used to iterate through the rows in the data reader.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the data type of the specified column.
        /// </summary>
        /// <returns>
        /// The data type of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override System.Type GetFieldType(int ordinal)
        {
            var propertyElement = propertiesElement.Elements().ElementAt(ordinal);
            var typeAttribute = propertyElement.Attribute(StorageHttpConstants.Namespaces.MetaData + "type");
            
            //Default value
            if (null == typeAttribute) return typeof(String);
            var type = System.Type.GetType("System." + typeAttribute.Value.Replace("Edm.", String.Empty));
            return type;
        }

        /// <summary>
        /// Gets the value of the specified column as a single-precision floating point number.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>2</filterpriority>
        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a globally-unique identifier (GUID).
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override Guid GetGuid(int ordinal)
        {
            var propertyElement = propertiesElement.Elements().ElementAt(ordinal);
            var value = new Guid(propertyElement.Value);
            return value;
        }

        /// <summary>
        /// Gets the value of the specified column as a 16-bit signed integer.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>2</filterpriority>
        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a 32-bit signed integer.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as a 64-bit signed integer.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>2</filterpriority>
        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the name of the column, given the zero-based column ordinal.
        /// </summary>
        /// <returns>
        /// The name of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override string GetName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            var propertyName = aliasPattern.Replace(name, ""); 
            var propertyElement = propertiesElement.Element(StorageHttpConstants.Namespaces.DataServices + propertyName);
            return propertyElement == null ? -1 : propertyElement.ElementsBeforeSelf().Count();
        }

        /// <summary>
        /// Returns a <see cref="T:System.Data.DataTable"/> that describes the column metadata of the <see cref="T:System.Data.Common.DbDataReader"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.DataTable"/> that describes the column metadata.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override System.Data.DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="T:System.String"/>.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all attribute columns in the collection for the current row.
        /// </summary>
        /// <returns>
        /// The number of instances of <see cref="T:System.Object"/> in the array.
        /// </returns>
        /// <param name="values">An array of <see cref="T:System.Object"/> into which to copy the attribute columns.
        ///                 </param><filterpriority>1</filterpriority>
        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="T:System.Data.Common.DbDataReader"/> contains one or more rows.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Data.Common.DbDataReader"/> contains one or more rows; otherwise false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool HasRows
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Data.Common.DbDataReader"/> is closed.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Data.Common.DbDataReader"/> is closed; otherwise false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value that indicates whether the column contains nonexistent or missing values.
        /// </summary>
        /// <returns>
        /// true if the specified column is equivalent to <see cref="T:System.DBNull"/>; otherwise false.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override bool IsDBNull(int ordinal)
        {
            if (ordinal < 0 || ordinal > propertiesElement.Elements().Count()) return true;

            var propertyElement = propertiesElement.Elements().ElementAt(ordinal);
            return propertyElement == null || String.IsNullOrEmpty(propertyElement.Value);
        }

        /// <summary>
        /// Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>
        /// true if there are more result sets; otherwise false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Advances the reader to the next record in a result set.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool Read()
        {
            if(entryElementsEnumerator == null)
            {
                response = request.Send();
                if (response == null) return false;

                data = response.Body;

                if (data.Root != null)
                {
                    var entryElements = data.Root.Elements(StorageHttpConstants.Namespaces.Atom + "entry");
                    entryElementsEnumerator = entryElements.GetEnumerator();
                }
            }
            if (data.Root != null && entryElementsEnumerator != null)
            {
                var moveNext = entryElementsEnumerator.MoveNext();
                if (entryElementsEnumerator.Current != null)
                {
                    var contentElement = entryElementsEnumerator.Current.Element(StorageHttpConstants.Namespaces.Atom + "content");
                    if (contentElement != null)
                        propertiesElement = contentElement.Element(StorageHttpConstants.Namespaces.MetaData + "properties");

                    var etagAttribute = entryElementsEnumerator.Current.Attribute(StorageHttpConstants.Namespaces.MetaData + "etag");
                    var titleElement = data.Root.Element(StorageHttpConstants.Namespaces.Atom + "title");

                    var etag = etagAttribute != null ? etagAttribute.Value : "*";
                    var tableName = titleElement != null ? titleElement.Value : "";
                    var rowKey = this["RowKey"].ToString();
                    var partitionKey = this["PartitionKey"].ToString();

                    Tracker.TrackEtagFor(etag, tableName, rowKey, partitionKey);
                }

                if(!moveNext && (response.NextRowKey != null || response.NextPartitionKey != null))
                {
                    entryElementsEnumerator = null;
                    request.NextRowKey = response.NextRowKey;
                    request.NextPartitionKey = response.NextPartitionKey;
                    return Read();
                }

                return moveNext;
            }
            return false;
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement. 
        /// </summary>
        /// <returns>
        /// The number of rows changed, inserted, or deleted. -1 for SELECT statements; 0 if no rows were affected or the statement failed.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the ETagTracker
        /// </summary>
        public ETagTracker Tracker{get; private set; }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="name">The name of the column.
        ///                 </param><filterpriority>1</filterpriority>
        public override object this[string name]
        {
            get { return this[GetOrdinal(name)]; }
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The value of the specified column.
        /// </returns>
        /// <param name="ordinal">The zero-based column ordinal.
        ///                 </param><filterpriority>1</filterpriority>
        public override object this[int ordinal]
        {
            get {
                var propertyElement = propertiesElement.Elements().ElementAt(ordinal);
                return propertyElement.Value;
            }
        }
    }
}