namespace B1PP.Database
{
    using System;

    public interface IMasterDataRecord
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        string Code { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets the master data entry.
        /// </summary>
        /// <value>
        /// The master data entry.
        /// </value>
        int? DocEntry { get; }

        /// <summary>
        /// Gets the canceled.
        /// </summary>
        /// <value>
        /// The canceled.
        /// </value>
        string Cancelled { get; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        string Object { get; }

        /// <summary>
        /// Gets the log instance.
        /// </summary>
        /// <value>
        /// The log instance.
        /// </value>
        int? LogInstance { get; }

        /// <summary>
        /// Gets the user signature.
        /// </summary>
        /// <value>
        /// The user signature.
        /// </value>
        int? UserSignature { get; }

        /// <summary>
        /// Gets the transferred.
        /// </summary>
        /// <value>
        /// The transferred.
        /// </value>
        string Transferred { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        string Status { get; }

        /// <summary>
        /// Gets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        DateTime? CreateDate { get; }

        /// <summary>
        /// Gets the create time.
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        int? CreateTime { get; }

        /// <summary>
        /// Gets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        DateTime? UpdateDate { get; }

        /// <summary>
        /// Gets the update time.
        /// </summary>
        /// <value>
        /// The update time.
        /// </value>
        int? UpdateTime { get; }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        string DataSource { get; }
    }
}