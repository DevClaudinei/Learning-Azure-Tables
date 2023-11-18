using System;
using Azure;
using Azure.Data.Tables;

namespace Azure_Tables.Models
{
    public class Contact : ITableEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}