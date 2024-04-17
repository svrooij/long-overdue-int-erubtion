using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Functions.IntBug;

public class CreateItem
{
    private readonly ILogger<CreateItem> _logger;

    public CreateItem(ILogger<CreateItem> logger)
    {
        _logger = logger;
    }

    [Function(nameof(CreateItem))]
    public async Task<MultiResponse> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {

        var id = Guid.NewGuid().ToString();

        _logger.LogInformation("Creating an item with id {id}", id);
        // This creates an item in the table storage and returns the item in the response
        // I guess this does an upsert in the background, but I cannot find any documentation or source code on this
        var response = new MultiResponse {
            Response = req.CreateResponse(HttpStatusCode.Created),
            Entity = new Models.OopsEntity
            {
                PartitionKey = "pk",
                RowKey = id,
                Oops = 42
            }
        };

        await response.Response.WriteAsJsonAsync(response.Entity);
        return response;

    }

    // This is the documented way to return multiple outputs
    // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=windows&wt.mc_id=SEC-MVP-5004985#multiple-output-bindings
    public class MultiResponse
    {
        [TableOutput("Oops", Connection = "AzureWebJobsStorage")]
        public Models.OopsEntity? Entity { get; set; }
        public required HttpResponseData Response { get; set; }
    }
}
