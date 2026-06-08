# Azure Function - Enrichment Summary

Repositorio da etapa 5 da arquitetura distribuida do PJBL, responsavel por uma **Azure Function HTTP Trigger** usada pelo BFF para enriquecimento de dados no endpoint agregado.

## Objetivo

Esta Function recebe contagens resumidas do BFF e devolve um payload de enriquecimento simples, servindo como demonstracao do componente serverless da arquitetura.

## Stack

- .NET 8 isolated worker
- Azure Functions v4
- HTTP Trigger
- Application Insights

## Function implementada

- `EnrichmentSummaryFunction`

### Rota

- `GET /api/enrichment-summary`

### Query params

- `peopleCount`
- `documentsCount`

### Exemplo de chamada

```text
http://localhost:7071/api/enrichment-summary?peopleCount=5&documentsCount=8
```

### Exemplo de resposta

```json
{
  "message": "Ha mais documentos do que colaboradores cadastrados.",
  "totalPeople": 5,
  "totalDocuments": 8,
  "generatedAtUtc": "2026-06-08T22:00:00Z",
  "source": "azure-function"
}
```

## Como rodar localmente

### Pre-requisitos

- .NET 8 SDK
- Azure Functions Core Tools

### Execucao

```bash
dotnet restore
func start
```

Se preferir:

```bash
dotnet build
func start
```

## Configuracao local

O arquivo `local.settings.json` deve conter:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```

## Integracao com o BFF

No BFF, a configuracao deve apontar para:

```json
"FunctionBaseUrl": "http://localhost:7071/",
"FunctionSummaryPath": "api/enrichment-summary"
```

E o mock deve ser desligado:

```json
"UseFunctionMocks": false
```

## Publicacao no Azure

Function App criada:

- `func-pjbl-arquitetura`

Dominio:

- `https://func-pjbl-arquitetura-dpeye5c5cyd9gthe.centralus-01.azurewebsites.net`

## Alunos

Preencher com os nomes do grupo.
