# Arlequim Stock 

Este projeto simula uma plataforma de controle de estoque e gerenciamento de produtos, com foco em boas práticas de arquitetura de software, testes automatizados e uso de Docker.

---

## 📌 Visão Geral

A aplicação é composta por uma Web API:

### 1. Arlequim.Stock
Responsável por:
- Autenticação via JWT
- Criação, listagem, detalhes e exclusão de produtos
- Inserção e consulta de estoque
- Criação e listagem de pedidos

---

## 🧰 Tecnologias Utilizadas

- ✅ .NET 9 + C#
- ✅ ASP.NET Core
- ✅ Entity Framework Core
- ✅ SQL Server (via Docker)
- ✅ Clean Architecture
- ✅ Clean Architecture + práticas de DDD + SOLID
- ✅ Testes: xUnit + Moq + FluentAssertions
- ✅ Docker + Docker Compose

---

## 🐳 Como Executar o Projeto

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)

> ℹ️ **Nota Importante:** Não é necessário instalar o SQL Server localmente.  
> O banco de dados sobe automaticamente via Docker Compose.  
> Caso queira rodar sem Docker, será necessário ter o SQL Server instalado e configurar a connection string manualmente em `appsettings.Development.json`.

### Executando os Serviços

1. **Clone o repositório:**

```bash
git clone https://github.com/AlisonMartins01/Arlequim.Stock.git
cd Arlequim.Stock
```

2. **Suba os serviços com Docker Compose:**

```bash
# Iniciar os serviços em background
docker compose up -d

# Visualizar logs em tempo real
docker compose logs -f

# Verificar status dos serviços
docker compose ps
```

3. **Acesse a aplicação:**

- **Arlequim.Stock:** [http://localhost:5089/swagger](http://localhost:5089/swagger)

---

## 🧪 Executando os Testes

```bash
# Executar todos os testes unitários
dotnet test
```

Os testes cobrem a camada de autenticação e a camada de pedidos, utilizando mocks com `Moq` e verificação de respostas HTTP.

---

## 📂 Estrutura de Pastas

```
/Arlequim.Stock
  ├── /Arlequim.Stock.API
  ├── /Arlequim.Stock.Application
  ├── /Arlequim.Stock.Domain
  ├── /Arlequim.Stock.Infrastructure
  └── /Arlequim.Stock.Tests
/docker-compose.yml
```

---

## 📄 Principais Endpoints

### 🔹 Autenticação (`/auth`)

| Método | Endpoint    | Descrição                                    |
|--------|-------------|----------------------------------------------|
| POST   | `/register` | Criar um novo usuário                        |
| POST   | `/login`    | Realizar login e retornar um Bearer Token    |

### 🔹 Produtos (`/products`)

| Método | Endpoint         | Descrição                                     |
|--------|------------------|-----------------------------------------------|
| POST   | `/`              | Criar um novo produto                         |
| PUT    | `/`              | Atualizar informações de um produto existente |
| DELETE | `/`              | Deletar um produto pelo ID                    |
| GET    | `/{productId}`   | Obter detalhes de um produto pelo ID          |
| GET    | `/`              | Listar todos os produtos existentes           |

### 🔹 Estoque (`/stock`)

| Método | Endpoint         | Descrição                                     |
|--------|------------------|-----------------------------------------------|
| POST   | `/`              | Adicionar estoque a um produto                |
| GET    | `/{productId}`   | Buscar estoque de um produto pelo ID          |

### 🔹 Pedidos (`/orders`)

| Método | Endpoint      | Descrição                                                |
|--------|---------------|----------------------------------------------------------|
| POST   | `/`           | Criar pedido e baixar estoque dos produtos utilizados   |
| GET    | `/my-orders`  | Listar pedidos do usuário autenticado*                  |

> *Não é necessário passar parâmetros, pois a listagem é feita através do ID do usuário armazenado no token JWT.

---


## 👤 Autor

Desenvolvido por **Alison Martins**  
[GitHub](https://github.com/AlisonMartins01) • [LinkedIn](https://www.linkedin.com/in/alison-martins-9785aa186/)

---

## 📝 Licença

Este projeto foi desenvolvido exclusivamente para fins de avaliação técnica.
