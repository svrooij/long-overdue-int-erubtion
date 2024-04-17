using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.IntBug.Models;

public class OopsEntity : ITableEntity
{
    private DateTimeOffset? _timestamp;
    private ETag _eTag;
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get => _timestamp; set => _timestamp = value; }
    public ETag ETag { get => _eTag; set => _eTag = value; }
    public int Oops { get; set; }
}
