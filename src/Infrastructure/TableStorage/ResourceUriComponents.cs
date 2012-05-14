namespace NHibernate.Drivers.Azure.TableStorage
{
    /// <summary>
    /// This type represents the different constituent parts that make up a resource Uri in the context of cloud services.
    /// </summary>
    public class ResourceUriComponents
    {
        /// <summary>
        /// The account name in the URI.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// This is really the first component (delimited by '/') after the account name. Since it happens to
        /// be a container name in the context of all our storage services (containers in blob storage,
        /// queues in the queue service and table names in table storage), it's named as ContainerName to make it more 
        /// readable at the cost of slightly being incorrectly named.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// The remaining string in the URI.
        /// </summary>
        public string RemainingPart { get; set; }

        /// <summary>
        /// Construct a ResourceUriComponents object.
        /// </summary>
        /// <param name="accountName">The account name that should become part of the URI.</param>
        /// <param name="containerName">The container name (container, queue or table name) that should become part of the URI.</param>
        /// <param name="remainingPart">Remaining part of the URI.</param>
        public ResourceUriComponents(string accountName, string containerName, string remainingPart)
        {
            AccountName = accountName;
            ContainerName = containerName;
            RemainingPart = remainingPart;
        }

        /// <summary>
        /// Construct a ResourceUriComponents object.        
        /// </summary>
        /// <param name="accountName">The account name that should become part of the URI.</param>
        /// <param name="containerName">The container name (container, queue or table name) that should become part of the URI.</param>
        public ResourceUriComponents(string accountName, string containerName)
            : this(accountName, containerName, null)
        {
        }

        /// <summary>
        /// Construct a ResourceUriComponents object.        
        /// </summary>
        /// <param name="accountName">The account name that should become part of the URI.</param>
        public ResourceUriComponents(string accountName)
            : this(accountName, null, null)
        {
        }

        /// <summary>
        /// Construct a ResourceUriComponents object.        
        /// </summary>
        public ResourceUriComponents()
        {
        }
    }
}
