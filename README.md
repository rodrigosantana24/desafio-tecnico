
---

# 🛒 Ecommerce Microservices - Desafio Técnico

Um ecossistema de microserviços em .NET 8, com API Gateway, comunicação assíncrona via RabbitMQ, e Docker Compose.

---

## 📌 Visão Geral

Este projeto simula a arquitetura de um pequeno ecommerce, dividido em três serviços independentes:

* **Gateway** - ponto de entrada do sistema, responsável pelo roteamento.

* **Vendas** - criação e listagem de pedidos.

* **Estoque** - gerenciamento de produtos.

A comunicação assíncrona é feita via RabbitMQ, permitindo troca de eventos entre os serviços.

---

## Estrutura do Repositório

```
/Ecommerce.Gateway       → API Gateway (Ocelot)
/Ecommerce.Vendas        → Microserviço de Vendas
/Ecommerce.Estoque       → Microserviço de Estoque
```


---

## 🎯Resultados das Requisições


```md
![Lista de Produtos](/Images/requisicao.png)
```

---

## ⚙️ Detalhes dos Serviços

### **📦 Estoque — `Ecommerce.Estoque`**

Gerencia produtos e disponibiliza endpoints de CRUD.
Arquivos principais:

* `Controllers/ProductsController.cs`
* `Data/EstoqueContext.cs`

---

### **🧾 Vendas — `Ecommerce.Vendas`**

Criação e listagem de pedidos.
Principais arquivos:

* `Controllers/OrdersController.cs`
* `Data/VendasContext.cs`

---

### **🚪 Gateway — `Ecommerce.Gateway`**

Roteamento usando **Ocelot**.
Configurações importantes:

* `ocelot.json`
* `Program.cs`

---

### **📨 Comunicação Assíncrona (RabbitMQ)**

* Produtor e consumidor de eventos entre Vendas e Estoque.
* Implementação:

  * `Services/RabbitMqService.cs`
  * `Services/RabbitMqConsumer.cs`

---

## ▶️ Como Rodar o Projeto

### **1. Pré-requisitos**

* Docker + Docker Compose
  *(ou .NET 8 SDK caso queira rodar sem containers)*

---

### **2. Rodando com Docker Compose (recomendado)**

```bash
cd path/to/repo
docker-compose up --build
```

---

### **3. Rodando manualmente (sem Docker)**

```bash
# Gateway
cd Ecommerce.Gateway
dotnet run

# Vendas
cd ../Ecommerce.Vendas
dotnet run

# Estoque
cd ../Ecommerce.Estoque
dotnet run
```

---

### **4. Testes rápidos**

Use:

* Arquivos `.http` incluídos no repositório
* Ou ferramentas como **Postman / Insomnia / VS Code REST Client**

---

## 🔗 Endpoints Principais

Conferir detalhes diretamente nos controllers:

* **Produtos (Estoque)**
  `Ecommerce.Estoque/Controllers/ProductsController.cs`

* **Pedidos (Vendas)**
  `Ecommerce.Vendas/Controllers/OrdersController.cs`

O **Gateway** roteia tudo via `ocelot.json`.


## 🧩 O que foi implementado

* Arquitetura de microserviços em .NET 8
* API Gateway usando Ocelot
* Comunicação assíncrona com RabbitMQ (producer/consumer)
* Dockerfile para cada serviço + docker-compose
* Arquivos `.http` para testes manuais

---

## 🚀 Próximos Passos / Contribuição

* Criar testes unitários e de integração
* Adicionar exemplos de request/response no README
* Inserir screenshots reais na pasta `Images/`
* Criar coleção Postman / Insomnia

---

## 👤 Autor

Repositório: **desafio-tecnico**
Se quiser, posso gerar imagens de exemplo ou criar um arquivo de coleção do Postman — só pedir!

---

Se quiser ajustar o tom (mais técnico, mais informal, mais corporativo), só me dizer!
