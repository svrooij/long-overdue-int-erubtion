using Functions.IntBug.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Functions.IntBug;

public class GetItemById
{
    private readonly ILogger<GetItemById> _logger;

    public GetItemById(ILogger<GetItemById> logger)
    {
        _logger = logger;
    }

    [Function("GetItemById")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getitembyid/{itemId:guid}")] HttpRequestData req,
        string itemId,
        [TableInput("Oops", "pk", "{itemId}", Connection = "AzureWebJobsStorage")] OopsEntity? item
        )
    {
        _logger.LogInformation("Got item from storage");
        var resp = req.CreateResponse(item != null ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound);
        if (item != null)
        {
            await resp.WriteAsJsonAsync(item);
        }
        return resp;
    }
}
