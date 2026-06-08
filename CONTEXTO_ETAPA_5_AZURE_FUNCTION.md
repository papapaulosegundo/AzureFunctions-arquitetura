# Contexto da Etapa 5 - Azure Function

Este arquivo registra o contexto completo do que foi feito no item 5 da atividade para manter continuidade nas proximas conversas e na integracao com o BFF.

## Objetivo desta etapa

Implementar o componente serverless da arquitetura distribuida por meio de uma **Azure Function HTTP Trigger**, usada pelo BFF para enriquecer a resposta agregada.

## Papel da Function na arquitetura

Esta Function nao substitui os microservicos. Ela complementa o BFF com uma operacao de:

- calculo
- enriquecimento
- resumo sintetico

No fluxo da aplicacao:

1. o BFF consulta `people`
2. o BFF consulta `documents`
3. o BFF envia os totais para a Function
4. a Function devolve um resumo enriquecido
5. o BFF agrega tudo em `/aggregated-data`

## Decisao adotada

Foi escolhida uma Function simples, explicavel e suficiente para a entrega:

- nome: `EnrichmentSummaryFunction`
- tipo: `HTTP Trigger`
- autenticacao: `Anonymous`
- rota: `api/enrichment-summary`

## Contrato implementado

### Entrada

Query params:

- `peopleCount`
- `documentsCount`

### Saida

Payload de resposta:

- `message`
- `totalPeople`
- `totalDocuments`
- `generatedAtUtc`
- `source`

## Regras de negocio simples

A Function implementa uma regra de enriquecimento leve:

- se ambos forem zero, informa que nao ha dados
- se houver mais documentos do que colaboradores, informa isso
- se houver mais colaboradores do que documentos, informa isso
- se houver equilibrio, retorna resumo padrao

## Infraestrutura Azure criada

Function App:

- `func-pjbl-arquitetura`

Dominio:

- `https://func-pjbl-arquitetura-dpeye5c5cyd9gthe.centralus-01.azurewebsites.net`

Runtime:

- Azure Functions v4
- .NET 8 isolated worker

Recursos de apoio:

- Resource Group: `rg-pjbl-arquitetura`
- Storage Account criada para a Function
- Application Insights habilitado

## Estrutura criada no repositorio

```text
AzureFunctions-arquitetura
|-- Functions
|   `-- EnrichmentSummaryFunction.cs
|-- Models
|   `-- EnrichmentSummaryResponse.cs
|-- Program.cs
|-- host.json
|-- local.settings.json
|-- local.settings.json.example
|-- README.md
|-- CONTEXTO_ETAPA_5_AZURE_FUNCTION.md
`-- AzureFunctions-arquitetura.csproj
```

## Como esta previsto o uso pelo BFF

No BFF, a configuracao local deve usar:

- `FunctionBaseUrl = http://localhost:7071/`
- `FunctionSummaryPath = api/enrichment-summary`
- `UseFunctionMocks = false`

Depois da publicacao no Azure, a URL base passara a ser o dominio da Function App.

## Validacao esperada desta etapa

Os testes esperados sao:

- subir a Function localmente
- chamar `GET /api/enrichment-summary`
- verificar resposta JSON
- integrar ao BFF
- chamar `GET /aggregated-data`

## Observacao final

Esta etapa fecha o terceiro pilar da arquitetura distribuida:

- microfrontend
- BFF
- microservicos
- serverless

Com isso, o endpoint agregado pode evoluir para consumir apenas servicos reais da arquitetura.
