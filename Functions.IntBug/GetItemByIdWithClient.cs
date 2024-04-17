using Azure.Data.Tables;
using Functions.IntBug.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Functions.IntBug;

public class GetItemByIdWithClient
{
    private readonly ILogger<GetItemByIdWithClient> _logger;

    public GetItemByIdWithClient(ILogger<GetItemByIdWithClient> logger)
    {
        _logger = logger;
    }

    [Function("GetItemByIdWithClient")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getitembyidclient/{itemId:guid}")] HttpRequestData req,
        string itemId,
        [TableInput("Oops", Connection = "AzureWebJobsStorage")] TableClient tableClient
        )
    {
        _logger.LogInformation("Getting item from storage");
        var item = await tableClient.GetEntityAsync<OopsEntity>("pk", itemId);
        _logger.LogInformation("Got item from storage");
        var resp = req.CreateResponse(item != null ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound);
        if (item != null)
        {
            await resp.WriteAsJsonAsync(item.Value);
        }
        return resp;
    }
}
