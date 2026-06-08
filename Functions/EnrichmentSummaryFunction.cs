using AzureFunctionsArquitetura.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace AzureFunctionsArquitetura.Functions;

public class EnrichmentSummaryFunction
{
    [Function("EnrichmentSummaryFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "enrichment-summary")] HttpRequestData req)
    {
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

        var peopleCountText = query["peopleCount"];
        var documentsCountText = query["documentsCount"];

        if (!int.TryParse(peopleCountText, out var peopleCount) || peopleCount < 0
            || !int.TryParse(documentsCountText, out var documentsCount) || documentsCount < 0)
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new
            {
                message = "Invalid query parameters. Use non-negative integers for peopleCount and documentsCount."
            });
            return badRequest;
        }

        var response = new EnrichmentSummaryResponse(
            Message: BuildMessage(peopleCount, documentsCount),
            TotalPeople: peopleCount,
            TotalDocuments: documentsCount,
            GeneratedAtUtc: DateTimeOffset.UtcNow,
            Source: "azure-function");

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(response);
        return ok;
    }

    private static string BuildMessage(int peopleCount, int documentsCount)
    {
        if (peopleCount == 0 && documentsCount == 0)
        {
            return "Nenhum dado encontrado para agregacao.";
        }

        if (documentsCount > peopleCount)
        {
            return "Ha mais documentos do que colaboradores cadastrados.";
        }

        if (peopleCount > documentsCount)
        {
            return "Ha mais colaboradores do que documentos cadastrados.";
        }

        return "Resumo enriquecido com sucesso.";
    }
}
