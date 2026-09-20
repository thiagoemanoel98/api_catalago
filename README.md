# API Catálogo

API REST para gerenciamento de produtos e categorias, desenvolvida com ASP.NET Core e PostgreSQL. O projeto implementa operações de cadastro, consulta, atualização e exclusão, além de paginação, validação de dados, tratamento de exceções e documentação interativa.

## Funcionalidades

- CRUD de produtos e categorias;
- consulta de produtos por categoria;
- paginação de resultados com metadados no cabeçalho `X-Pagination`;
- atualização parcial de produtos com HTTP PATCH;
- validação dos dados de entrada;
- mapeamento entre entidades e DTOs com AutoMapper;
- acesso a dados com Repository e Unit of Work;
- tratamento centralizado de exceções e logs;
- documentação interativa com OpenAPI e Swagger UI;
- PostgreSQL configurado por Docker Compose.

## Tecnologias

- .NET 9 e ASP.NET Core;
- C#;
- Entity Framework Core;
- PostgreSQL;
- Npgsql;
- AutoMapper;
- Newtonsoft.Json;
- OpenAPI e Swagger UI;
- Docker e Docker Compose.

## Organização do projeto

```text
ApiCatalago/
├── Context/         # Contexto do Entity Framework Core
├── Controllers/     # Endpoints de produtos e categorias
├── DTOs/            # Objetos de entrada, saída e mapeamentos
├── Extensions/      # Extensões e tratamento de exceções
├── Filters/         # Filtros de exceção e logging
├── Logging/         # Provedor de logs personalizado
├── Migrations/      # Migrações do banco de dados
├── Models/          # Entidades do domínio
├── Pagination/      # Parâmetros e resultados paginados
├── Repositories/    # Repository e Unit of Work
└── Services/        # Serviços da aplicação
```

## Pré-requisitos

- [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0);
- [Docker](https://docs.docker.com/get-docker/) com Docker Compose;
- ferramenta `dotnet-ef` para aplicar as migrações.

Caso ainda não tenha a ferramenta instalada:

```bash
dotnet tool install --global dotnet-ef
```

## Como executar

Clone o repositório:

```bash
git clone https://github.com/thiagoemanoel98/api_catalago.git
cd api_catalago/ApiCatalago
```

Inicie o PostgreSQL:

```bash
docker compose up -d
```

Restaure as dependências e aplique as migrações:

```bash
dotnet restore
dotnet ef database update
```

Execute a API:

```bash
dotnet run
```

Por padrão, a aplicação será disponibilizada em:

- API: `http://localhost:5165`
- Swagger UI: `http://localhost:5165/swagger`

> A configuração incluída no repositório é destinada exclusivamente ao ambiente local. Em outros ambientes, configure a string de conexão por variável de ambiente ou secret manager.

## Endpoints

### Categorias

| Método | Endpoint | Descrição |
| --- | --- | --- |
| `GET` | `/categories` | Lista todas as categorias |
| `GET` | `/categories/{id}` | Consulta uma categoria pelo identificador |
| `GET` | `/categories/pagination?pageNumber=1&pageSize=10` | Lista categorias com paginação |
| `POST` | `/categories` | Cadastra uma categoria |
| `PUT` | `/categories/{id}` | Atualiza uma categoria |
| `DELETE` | `/categories/{id}` | Remove uma categoria |

### Produtos

| Método | Endpoint | Descrição |
| --- | --- | --- |
| `GET` | `/products` | Lista todos os produtos |
| `GET` | `/products/{id}` | Consulta um produto pelo identificador |
| `GET` | `/products/products/{categoryId}` | Lista produtos de uma categoria |
| `GET` | `/products/pagination?pageNumber=1&pageSize=10` | Lista produtos com paginação |
| `POST` | `/products` | Cadastra um produto |
| `PUT` | `/products/{id}` | Atualiza todos os dados de um produto |
| `PATCH` | `/products/{id}/UpdatePartial` | Atualiza parcialmente um produto |
| `DELETE` | `/products/{id}` | Remove um produto |

O tamanho máximo de uma página é de 50 registros. As consultas paginadas retornam informações como página atual, quantidade total de páginas e disponibilidade das páginas anterior e seguinte no cabeçalho `X-Pagination`.

## Exemplos de requisição

Criar uma categoria:

```http
POST /categories
Content-Type: application/json

{
  "name": "Eletrônicos",
  "imageUrl": "https://exemplo.com/eletronicos.png"
}
```

Criar um produto:

```http
POST /products
Content-Type: application/json

{
  "categoryId": 1,
  "name": "Fone de ouvido",
  "description": "Fone de ouvido sem fio",
  "price": 199.90,
  "imageUrl": "https://exemplo.com/fone.png"
}
```

Atualizar parcialmente um produto:

```http
PATCH /products/1/UpdatePartial
Content-Type: application/json-patch+json

[
  {
    "op": "replace",
    "path": "/price",
    "value": 179.90
  }
]
```

## Encerrar o ambiente local

Para interromper os contêineres:

```bash
docker compose down
```

Para também remover o volume do banco de dados:

```bash
docker compose down -v
```
