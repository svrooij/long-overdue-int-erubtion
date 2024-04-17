using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Functions.IntBug;

public class CreateItemWithClient
{
    private readonly ILogger<CreateItemWithClient> _logger;

    public CreateItemWithClient(ILogger<CreateItemWithClient> logger)
    {
        _logger = logger;
    }

    [Function(nameof(CreateItemWithClient))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        // It is called "TableInput" but this is the way to get the tableclient
        [TableInput("Oops", Connection = "AzureWebJobsStorage")] TableClient tableClient
        )
    {

        var id = Guid.NewGuid().ToString();
        var entity = new Models.OopsEntity
        {
            PartitionKey = "pk",
            RowKey = id,
            Oops = 42
        };

        _logger.LogInformation("Creating an item with id {id}", id);

        // Doing it this way creates the correct item in the table storage
        // No clue how this is different from the other way
        await tableClient.AddEntityAsync(entity);
        var response = req.CreateResponse(HttpStatusCode.Created);

        await response.WriteAsJsonAsync(entity);
        return response;

    }
}
