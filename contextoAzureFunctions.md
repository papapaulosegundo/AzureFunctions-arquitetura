# Contexto da Azure Function

## Papel na entrega

Esta Azure Function representa o componente serverless da arquitetura e e consumida pelo BFF no endpoint `GET /aggregated-data`.

Fluxo esperado:

1. o BFF consulta os microservicos de `people` e `documents`
2. o BFF envia os totais para a Function
3. a Function devolve um resumo enriquecido
4. o BFF retorna tudo em uma unica resposta

## Endpoint da Function

- nome: `EnrichmentSummaryFunction`
- tipo: `HTTP Trigger`
- autenticacao: `Anonymous`
- rota: `GET /api/enrichment-summary`

## Contrato

Entrada por query string:

- `peopleCount`
- `documentsCount`

Saida JSON:

- `message`
- `totalPeople`
- `totalDocuments`
- `generatedAtUtc`
- `source`

Se os parametros nao forem inteiros nao negativos, a Function retorna `400 Bad Request`.

## Conexao com o BFF

Configuracao local:

- `FunctionBaseUrl = http://localhost:7071/`
- `FunctionSummaryPath = api/enrichment-summary`
- `UseFunctionMocks = false`

Configuracao publicada:

- `FunctionBaseUrl = https://func-pjbl-arquitetura-dpeye5c5cyd9gthe.centralus-01.azurewebsites.net/`
- `FunctionSummaryPath = api/enrichment-summary`
