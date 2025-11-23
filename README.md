
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

![Lista de Produtos](/Images/requisicao.png)


---

## ⚙️ Detalhes da API

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

## Como Rodar o Projeto

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

## 👤 Autor

**Rodrigo Santana**

[LinkedIn](https://www.linkedin.com/in/rodrigo-santana-280928233/)

---

