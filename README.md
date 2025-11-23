
# Ecommerce Microservices - Inventory & Sales

**Resumo:** Projeto demonstrando arquitetura de microserviços em .NET 8: microserviço de Estoque (`Ecommerce.Estoque`), microserviço de Vendas (`Ecommerce.Vendas`) e API Gateway (`Ecommerce.Gateway`). Comunicação assíncrona via RabbitMQ, persistência com SQLite e roteamento com Ocelot.

**Arquitetura (visão rápida):**
- **Inventory (Estoque):** gerencia produtos e estoque; consome eventos `order-created-queue` do RabbitMQ para debitar estoque.
- **Sales (Vendas):** cria pedidos, valida estoque via requisição HTTP para Inventory e publica evento de pedido criado no RabbitMQ.
- **API Gateway:** Ocelot expondo rotas consolidadas para os dois serviços.

**Imagem (coloque arquivos em `images/`):**
- Arquitetura: `images/architecture.png`  
- Fluxo de criação de pedido: `images/flow.png`  
- Screenshots da UI / Postman: `images/screenshots.png`

**Como executar (Docker - recomendado)**
- Certifique-se de ter o Docker Desktop em execução.
- No PowerShell, na raiz do projeto, execute:

```
docker compose up --build -d
```

- Serviços expostos (host -> container):
	- Gateway: `http://localhost:5000`
	- Inventory (Estoque): `http://localhost:5001`
	- Sales (Vendas): `http://localhost:5002`
	- RabbitMQ Management: `http://localhost:15672` (user: `guest` / pass: `guest`)

**Como executar localmente (sem Docker)**
- Abra as pastas de projeto e rode via `dotnet run` em cada projeto:
	- `Ecommerce.Estoque` (porta 5001)
	- `Ecommerce.Vendas` (porta 5002)
	- `Ecommerce.Gateway` (porta 5000)

**Teste rápido (PowerShell script)**
- Use o script `test_flow.ps1` na raiz para executar um fluxo de teste que:
	1. Cria um produto
	2. Cria um pedido
	3. Mostra produtos antes/depois e lista de pedidos
- Ele gera arquivos JSON em `./test-results/` para você capturar screenshots.

Exemplo de execução no PowerShell:

```
# Permitir execução do script se necessário
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass ; .\test_flow.ps1
```

**Endpoints principais**
- Inventory (`Ecommerce.Estoque`):
	- `GET /api/products` - lista produtos
	- `GET /api/products/{id}` - obtém produto
	- `POST /api/products` - cria produto
	- `PUT /api/products/{id}` - atualiza produto
	- `DELETE /api/products/{id}` - remove produto
- Sales (`Ecommerce.Vendas`):
	- `GET /api/orders` - lista pedidos
	- `POST /api/orders` - cria pedido (valida estoque e publica evento)

Quando usar o Gateway, prefixe com `/estoque` ou `/vendas`, por exemplo: `http://localhost:5000/estoque/api/products`.

**Notes / Troubleshooting**
- Se o Gateway retornar `502 Bad Gateway`, verifique se os serviços downstream estão up e escutando nas portas corretas.
- Verifique logs dos containers:

```
docker compose logs gateway --tail 200
docker compose logs estoque --tail 200
docker compose logs vendas --tail 200
```

- Se o Docker Desktop não estiver executando, o compose pode falhar com erro de pipe (Windows). Abra o Docker Desktop antes de rodar o `docker compose`.

**Próximos passos sugeridos (opcional)**
- Adicionar healthchecks e políticas de restart no `docker-compose.yml`.
- Tornar a conexão com RabbitMQ mais resiliente (retry/backoff) para evitar falhas na inicialização.

Se quiser, eu atualizo o README com textos para slides (foco na apresentação) ou gero uma collection do Postman para demonstrar o fluxo.

