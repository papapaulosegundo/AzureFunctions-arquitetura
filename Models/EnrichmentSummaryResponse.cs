namespace AzureFunctionsArquitetura.Models;

public record EnrichmentSummaryResponse(
    string Message,
    int TotalPeople,
    int TotalDocuments,
    DateTimeOffset GeneratedAtUtc,
    string Source);
