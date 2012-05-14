using System.Xml.Linq;

namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// Constants to be used in the requests to azure
    /// </summary>
    public class StorageHttpConstants
    {

        public class Namespaces
        {
            public static XNamespace DataServices = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            public static XNamespace MetaData = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            public static XNamespace Atom = "http://www.w3.org/2005/Atom";
        }

        public static class ConstChars
        {
            public const string Linefeed = "\n";
            public const string CarriageReturnLinefeed = "\r\n";
            public const string Colon = ":";
            public const string Comma = ",";
            public const string Slash = "/";
            public const string BackwardSlash = @"\";
            public const string Space = " ";
            public const string Ampersand = "&";
            public const string QuestionMark = "?";
            public const string Equal = "=";
            public const string Bang = "!";
            public const string Star = "*";
            public const string Dot = ".";
        }

        public static class RequestParams
        {
            public const string NumOfMessages = "numofmessages";
            public const string VisibilityTimeout = "visibilitytimeout";
            public const string PeekOnly = "peekonly";
            public const string MessageTtl = "messagettl";
            public const string Messages = "messages";
            public const string PopReceipt = "popreceipt";
        }

        public static class QueryParams
        {
            public const string SeparatorForParameterAndValue = "=";
            public const string QueryParamTimeout = "timeout";
            public const string QueryParamComp = "comp";

            // Other query string parameter names
            public const string QueryParamBlockId = "blockid";
            public const string QueryParamPrefix = "prefix";
            public const string QueryParamMarker = "marker";
            public const string QueryParamMaxResults = "maxresults";
            public const string QueryParamDelimiter = "delimiter";
            public const string QueryParamModifiedSince = "modifiedsince";
        }

        public static class CompConstants
        {
            public const string Metadata = "metadata";
            public const string List = "list";
            public const string BlobList = "bloblist";
            public const string BlockList = "blocklist";
            public const string Block = "block";
            public const string Acl = "acl";
        }

        public static class XmlElementNames
        {
            public const string BlockList = "BlockList";
            public const string Block = "Block";
            public const string EnumerationResults = "EnumerationResults";
            public const string Prefix = "Prefix";
            public const string Marker = "Marker";
            public const string MaxResults = "MaxResults";
            public const string Delimiter = "Delimiter";
            public const string NextMarker = "NextMarker";
            public const string Containers = "Containers";
            public const string Container = "Container";
            public const string ContainerName = "Name";
            public const string ContainerNameAttribute = "ContainerName";
            public const string AccountNameAttribute = "AccountName";
            public const string LastModified = "LastModified";
            public const string Etag = "Etag";
            public const string Url = "Url";
            public const string CommonPrefixes = "CommonPrefixes";
            public const string ContentType = "ContentType";
            public const string ContentEncoding = "ContentEncoding";
            public const string ContentLanguage = "ContentLanguage";
            public const string Size = "Size";
            public const string Blobs = "Blobs";
            public const string Blob = "Blob";
            public const string BlobName = "Name";
            public const string BlobPrefix = "BlobPrefix";
            public const string BlobPrefixName = "Name";
            public const string Name = "Name";
            public const string Queues = "Queues";
            public const string Queue = "Queue";
            public const string QueueName = "QueueName";
            public const string QueueMessagesList = "QueueMessagesList";
            public const string QueueMessage = "QueueMessage";
            public const string MessageId = "MessageId";
            public const string PopReceipt = "PopReceipt";
            public const string InsertionTime = "InsertionTime";
            public const string ExpirationTime = "ExpirationTime";
            public const string TimeNextVisible = "TimeNextVisible";
            public const string MessageText = "MessageText";

            // Error specific constants
            public const string ErrorRootElement = "Error";
            public const string ErrorCode = "Code";
            public const string ErrorMessage = "Message";
            public const string ErrorException = "ExceptionDetails";
            public const string ErrorExceptionMessage = "ExceptionMessage";
            public const string ErrorExceptionStackTrace = "StackTrace";
            public const string AuthenticationErrorDetail = "AuthenticationErrorDetail";

            //The following are for table error messages
            public const string DataWebMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            public const string TableErrorCodeElement = "code";
            public const string TableErrorMessageElement = "message";
        }

        public static class HeaderNames
        {
            public const string PrefixForStorageProperties = "x-ms-prop-";
            public const string PrefixForMetadata = "x-ms-meta-";
            public const string PrefixForStorageHeader = "x-ms-";
            public const string PrefixForTableContinuation = "x-ms-continuation-";

            //
            // Standard headers...
            //
            public const string ContentLanguage = "Content-Language";
            public const string ContentLength = "Content-Length";
            public const string ContentType = "Content-Type";
            public const string ContentEncoding = "Content-Encoding";
            public const string ContentMD5 = "Content-MD5";
            public const string ContentRange = "Content-Range";
            public const string LastModifiedTime = "Last-Modified";
            public const string Server = "Server";
            public const string Allow = "Allow";
            public const string ETag = "ETag";
            public const string Range = "Range";
            public const string Date = "Date";
            public const string Authorization = "Authorization";
            public const string IfModifiedSince = "If-Modified-Since";
            public const string IfUnmodifiedSince = "If-Unmodified-Since";
            public const string IfMatch = "If-Match";
            public const string IfNoneMatch = "If-None-Match";
            public const string IfRange = "If-Range";
            public const string NextPartitionKey = "NextPartitionKey";
            public const string NextRowKey = "NextRowKey";
            public const string NextTableName = "NextTableName";

            //
            // Storage specific custom headers...
            //
            public const string StorageDateTime = PrefixForStorageHeader + "date";
            public const string PublicAccess = PrefixForStorageProperties + "publicaccess";
            public const string StorageRange = PrefixForStorageHeader + "range";

            public const string CreationTime = PrefixForStorageProperties + "creation-time";
            public const string ForceUpdate = PrefixForStorageHeader + "force-update";
            public const string ApproximateMessagesCount = PrefixForStorageHeader + "approximate-messages-count";
        }

        public static class HeaderValues
        {
            public const string ContentTypeXml = "application/xml";

            /// <summary>
            /// This is the default content-type xStore uses when no content type is specified
            /// </summary>
            public const string DefaultContentType = "application/octet-stream";

            // The Range header value is "bytes=start-end", both start and end can be empty
            public const string RangeHeaderFormat = "bytes={0}-{1}";

        }

        public static class AuthenticationSchemeNames
        {
            public const string SharedKeyAuthSchemeName = "SharedKey";
            public const string SharedKeyLiteAuthSchemeName = "SharedKeyLite";
        }

        public static class HttpMethod
        {
            public const string Get = "GET";
            public const string Put = "PUT";
            public const string Post = "POST";
            public const string Head = "HEAD";
            public const string Delete = "DELETE";
            public const string Trace = "TRACE";
            public const string Options = "OPTIONS";
            public const string Connect = "CONNECT";
            public const string Merge = "MERGE";
        }

        public static class BlobBlockConstants
        {
            public const int KB = 1024;
            public const int MB = 1024 * KB;
            /// <summary>
            /// When transmitting a blob that is larger than this constant, this library automatically
            /// transmits the blob as individual blocks. I.e., the blob is (1) partitioned
            /// into separate parts (these parts are called blocks) and then (2) each of the blocks is 
            /// transmitted separately.
            /// The maximum size of this constant as supported by the real blob storage service is currently 
            /// 64 MB; the development storage tool currently restricts this value to 2 MB.
            /// Setting this constant can have a significant impact on the performance for uploading or
            /// downloading blobs.
            /// As a general guideline: If you run in a reliable environment increase this constant to reduce
            /// the amount of roundtrips. In an unreliable environment keep this constant low to reduce the 
            /// amount of data that needs to be retransmitted in case of connection failures.
            /// </summary>
            public const long MaximumBlobSizeBeforeTransmittingAsBlocks = 2 * MB;
            /// <summary>
            /// The size of a single block when transmitting a blob that is larger than the 
            /// MaximumBlobSizeBeforeTransmittingAsBlocks constant (see above).
            /// The maximum size of this constant is currently 4 MB; the development storage 
            /// tool currently restricts this value to 1 MB.
            /// Setting this constant can have a significant impact on the performance for uploading or 
            /// downloading blobs.
            /// As a general guideline: If you run in a reliable environment increase this constant to reduce
            /// the amount of roundtrips. In an unreliable environment keep this constant low to reduce the 
            /// amount of data that needs to be retransmitted in case of connection failures.
            /// </summary>
            public const long BlockSize = 1 * MB;
        }

        public static class ListingConstants
        {
            public const int MaxContainerListResults = 100;
            public const int MaxBlobListResults = 100;
            public const int MaxQueueListResults = 50;
            public const int MaxTableListResults = 50;
        }

        /// <summary>
        /// Contains regular expressions for checking whether container and table names conform
        /// to the rules of the storage REST protocols.
        /// </summary>
        public static class RegularExpressionStrings
        {
            /// <summary>
            /// Container or queue names that match against this regular expression are valid.
            /// </summary>
            public const string ValidContainerNameRegex = @"^([a-z]|\d){1}([a-z]|-|\d){1,61}([a-z]|\d){1}$";

            /// <summary>
            /// Table names that match against this regular expression are valid.
            /// </summary>
            public const string ValidTableNameRegex = @"^([a-z]|[A-Z]){1}([a-z]|[A-Z]|\d){2,62}$";
        }

        public static class StandardPortalEndpoints
        {
            public const string BlobStorage = "blob";
            public const string QueueStorage = "queue";
            public const string TableStorage = "table";
            public const string StorageHostSuffix = ".core.windows.net";
            public const string BlobStorageEndpoint = BlobStorage + StorageHostSuffix;
            public const string QueueStorageEndpoint = QueueStorage + StorageHostSuffix;
            public const string TableStorageEndpoint = TableStorage + StorageHostSuffix;
        }
    }
}