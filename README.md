
# Ecommerce Microservices - Desafio Técnico

**Visão Geral:**
- **Projeto**: Conjunto de microserviços .NET (Gateway, Vendas, Estoque) com integração via RabbitMQ e orquestração via `docker-compose`.
- **Objetivo**: API Gateway para roteamento (Ocelot), serviços de Vendas e Estoque, e comunicação assíncrona com RabbitMQ.

**Estrutura do Repositório**
- `EcommerceMicroservices.sln`: Solução .NET com os projetos.
- `docker-compose.yml`: Orquestração das imagens/containeres para rodar localmente.
- `requests.http`: Coleção de requisições HTTP (útil para testes rápidos usando VS Code REST Client ou HTTP client similar).
- `Ecommerce.Gateway/`: Projeto do API Gateway (contém `ocelot.json`, `Program.cs`, `Dockerfile`).
- `Ecommerce.Vendas/`: Serviço de Vendas (endpoints para criar/listar pedidos, `RabbitMqService.cs`).
- `Ecommerce.Estoque/`: Serviço de Estoque (endpoints para produtos, `RabbitMqConsumer.cs`).
- `Images/`: Pasta para colocar imagens que ilustrem os resultados das requisições (atualmente vazia). Coloque capturas de tela aqui para incluí-las no README.

**Como o projeto está organizado (resumo)**
- **Gateway (Ecommerce.Gateway)**: Roteia requisições para os microsserviços usando Ocelot. Verifique `ocelot.json` e `Program.cs` para as rotas configuradas.
- **Vendas (Ecommerce.Vendas)**: Gerenciamento de pedidos — contém controller(s) em `Controllers/OrdersController.cs` e persistência em `Data/VendasContext.cs`.
- **Estoque (Ecommerce.Estoque)**: Gerenciamento de produtos — veja `Controllers/ProductsController.cs` e `Data/EstoqueContext.cs`.
- **Comunicação assíncrona**: Implementada via RabbitMQ — componentes em `Services/RabbitMqService.cs` e `Services/RabbitMqConsumer.cs`.

**Como rodar (local)**
1. Pré-requisitos: `docker` + `docker-compose` (ou .NET 8 SDK para rodar localmente sem containers).
2. Rodando com Docker Compose (recomendado):

```powershell
cd path\to\repo  # ex: cd C:\Users\...\desafio-tecnico
docker-compose up --build
```

3. Rodando individualmente (sem Docker):

```powershell
cd Ecommerce.Gateway
dotnet run
# em outro terminal
cd ../Ecommerce.Vendas
dotnet run
cd ../Ecommerce.Estoque
dotnet run
```

4. Testes rápidos: use os arquivos `*.http` incluídos (`requests.http`, `Ecommerce.*.http`) ou uma ferramenta como Postman / REST Client no VS Code.

**Endpoints importantes**
- Para rotas e verbos exatos confira os controllers:
	- `Ecommerce.Estoque/Controllers/ProductsController.cs`
	- `Ecommerce.Vendas/Controllers/OrdersController.cs`
- O Gateway aplica roteamento definido em `Ecommerce.Gateway/ocelot.json` — é o ponto de entrada se estiver usando o gateway.

**Resultados das requisições (Imagens)**
- A pasta `Images/` está atualmente vazia. Para adicionar imagens que mostrem os resultados das requisições (ex.: respostas JSON, logs ou telas do Postman), coloque os arquivos dentro de `Images/`.
- Exemplo de inclusão de imagem no README (Markdown):

```md
![Requisição - Lista de Produtos](Images/request-products.png)
```

- Recomendo gerar imagens para estas requisições-chave e salvá-las com nomes descritivos como `request-products.png`, `request-create-order.png`, `rabbitmq-event.png`.

**O que eu fiz / Notas do autor**
- Criei uma solução com três componentes principais: API Gateway (Ocelot), serviço de Vendas e serviço de Estoque.
- Adicionei integração com RabbitMQ para comunicação assíncrona entre serviços (produtor/consumidor).
- Adicionei `Dockerfile` em cada serviço e um `docker-compose.yml` para facilitar o deploy local.
- Incluí arquivos `*.http` para facilitar testes manuais das APIs.

Se quiser, posso:
- Adicionar imagens reais de exemplos de requisições na pasta `Images/` (envie as capturas ou permita que eu gere exemplos).
- Gerar um arquivo de Postman/Insomnia com as coleções de requisições a partir dos `*.http`.
- Documentar cada endpoint com exemplos de request/response diretamente no README.

**Como contribuir / próximos passos**
- Rodar o projeto localmente e testar os endpoints.
- Implementar testes automatizados (unit/integration).
- Documentar exemplos de payloads e códigos de resposta para cada endpoint.

**Contato / Autor**
- Repositório: `desafio-tecnico`
- Se quiser que eu adicione as imagens de resultado diretamente ao README, envie as imagens para a pasta `Images/` ou me permita gerar capturas com exemplos.

