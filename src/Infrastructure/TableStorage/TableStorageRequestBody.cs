using System;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Xml.Linq;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Represents the body of a table storage request
    /// </summary>
    public class TableStorageRequestBody : XDocument
    {
        /// <summary>
        /// Creates a new instance of TableStorageRequestBody
        /// </summary>
        public TableStorageRequestBody()
        {
            Declaration = new XDeclaration("1.0", "utf-8", "yes");

            Add(
                new XElement(StorageHttpConstants.Namespaces.Atom + "entry",
                    new XAttribute("xmlns", StorageHttpConstants.Namespaces.Atom),
                    new XAttribute(XNamespace.Xmlns + "d", StorageHttpConstants.Namespaces.DataServices),
                    new XAttribute(XNamespace.Xmlns + "m", StorageHttpConstants.Namespaces.MetaData),

                    new XElement(StorageHttpConstants.Namespaces.Atom + "title"),
                    new XElement(StorageHttpConstants.Namespaces.Atom + "updated", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ", CultureInfo.InvariantCulture)),
                    new XElement(StorageHttpConstants.Namespaces.Atom + "author",
                        new XElement(StorageHttpConstants.Namespaces.Atom + "name")
                    ),
                    new XElement(StorageHttpConstants.Namespaces.Atom + "id"),

                    new XElement(StorageHttpConstants.Namespaces.Atom + "content",
                        new XAttribute("type", "application/xml"),
                        new XElement(StorageHttpConstants.Namespaces.MetaData + "properties")
                    )
                )
            );
        }

        /// <summary>
        /// Add a property
        /// </summary>
        public TableStorageRequestBody AddProperty(string name, object value)
        {
            return AddProperty(name, value, DbType.String);
        }

        /// <summary>
        /// Add a property
        /// </summary>
        public TableStorageRequestBody AddProperty(string name, object value, DbType dbType)
        {
            var propertyElement = new XElement(StorageHttpConstants.Namespaces.DataServices + name, value);
            switch (dbType)
            {
                case DbType.String:
                    break;
                default:
                    propertyElement.Add(new XAttribute(StorageHttpConstants.Namespaces.MetaData + "type", "Edm." + dbType));
                    break;
            }
            ContentProperties.Add(propertyElement);

            return this;
        }

        private XElement ContentProperties
        {
            get { return Descendants(XName.Get("properties", StorageHttpConstants.Namespaces.MetaData.NamespaceName)).FirstOrDefault(); }
        }

        /// <summary>
        /// Converts the body to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Declaration + Environment.NewLine + Root;
        }
    }
}