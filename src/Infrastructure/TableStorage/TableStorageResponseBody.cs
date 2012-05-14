using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// The body of the response
    /// </summary>
    public class TableStorageResponseBody : XDocument
    {
        private IEnumerable<XElement> entries;
        
        /// <summary>
        /// Create a new instance of TableStorageResponseBody based on the content passed in
        /// </summary>
        /// <param name="content"></param>
        public TableStorageResponseBody(string content)
        {
            var doc = new XmlDocument();
            doc.LoadXml(content);
            using (var writer = CreateWriter())
            {
                doc.Save(writer);
            }
        }

        /// <summary>
        /// Get the entries in the body
        /// </summary>
        public IEnumerable<XElement> Entries
        {
            get
            {
               return entries = entries  ?? Root.Elements(StorageHttpConstants.Namespaces.Atom + "entry");
            }
        }
    }
}