# MentalCheck .NET - API de Monitoramento de Bem-estar

API RESTful desenvolvida em ASP.NET Core para monitoramento de bem-estar mental de trabalhadores em ambientes híbridos.

## 🎯 Visão Geral

O MentalCheck permite que trabalhadores realizem check-ins diários sobre seu estado emocional, registrando métricas como:
- Nível de estresse (1-10)
- Motivação (1-10)
- Cansaço físico (1-10)
- Satisfação com trabalho (1-10)
- Qualidade do sono (1-10)

A API oferece funcionalidades completas de CRUD, busca avançada com filtros, paginação, ordenação e HATEOAS.

## 🏗️ Arquitetura

### Estrutura do Projeto
```
MentalCheck.API/
├── Models/              # Entidades do banco de dados
├── DTOs/                # Data Transfer Objects
├── Data/                # DbContext e configurações do EF Core
├── Controllers/         # Endpoints da API
├── Extensions/          # Helpers e extensões
├── appsettings.json     # Configurações
└── Program.cs           # Ponto de entrada
```

### Tecnologias Utilizadas
- **.NET 8.0** - Framework
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core 8.0** - ORM
- **Oracle Database** - Banco de dados
- **Swagger/OpenAPI** - Documentação interativa

## 🚀 Como Rodar

### Pré-requisitos
- Visual Studio 2022 ou superior
- .NET 8.0 SDK
- Acesso ao banco Oracle da FIAP

### Passo 1: Clone o Repositório
```bash
git clone <URL_DO_REPOSITORIO>
cd MentalCheck.API
```

### Passo 2: Configure a Connection String
Edite o arquivo `appsettings.json` e insira o rm e a senha fornecida em arquivo a parte:
```json
"ConnectionStrings": {
  "Oracle": "User Id=SEU_RM_AQUI;Password=SUA_SENHA_AQUI;Data Source=..."
}
```

### Passo 3: Execute as Migrations
Abra o **Package Manager Console** no Visual Studio e execute:
```powershell
Add-Migration InitialCreate
Update-Database
```

Ou via CLI:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Passo 4: Execute o Projeto
No Visual Studio:
- Pressione `F5` ou clique em "Run"
- O Swagger abrirá automaticamente em `https://localhost:7XXX`

Via CLI:
```bash
dotnet run
```

## 📚 Endpoints da API

### Usuários

#### `GET /api/usuarios`
Lista todos os usuários.

**Response 200:**
```json
[
  {
    "id": 1,
    "nome": "Carlos Silva",
    "email": "carlos@empresa.com",
    "cargo": "Analista",
    "modalidadeTrabalho": "HIBRIDO",
    "dataCadastro": "2024-11-23T10:00:00",
    "links": [
      { "rel": "self", "href": "/api/usuarios/1", "method": "GET" },
      { "rel": "update", "href": "/api/usuarios/1", "method": "PUT" },
      { "rel": "delete", "href": "/api/usuarios/1", "method": "DELETE" }
    ]
  }
]
```

#### `GET /api/usuarios/{id}`
Busca usuário por ID.

#### `POST /api/usuarios`
Cria novo usuário.

**Request Body:**
```json
{
  "nome": "Maria Santos",
  "email": "maria@empresa.com",
  "senha": "senha123",
  "cargo": "Desenvolvedora",
  "modalidadeTrabalho": "REMOTO"
}
```

#### `PUT /api/usuarios/{id}`
Atualiza usuário existente.

#### `DELETE /api/usuarios/{id}`
Remove usuário.

#### `GET /api/usuarios/search`
Busca avançada com filtros, paginação e ordenação.

**Query Parameters:**
- `nome` - Filtrar por nome (contém)
- `email` - Filtrar por email (contém)
- `cargo` - Filtrar por cargo (contém)
- `modalidadeTrabalho` - Filtrar por modalidade (exato)
- `page` - Número da página (padrão: 1)
- `pageSize` - Tamanho da página (padrão: 10, máx: 100)
- `orderBy` - Campo para ordenar (nome, email, cargo, datacadastro)
- `direction` - Direção (asc, desc)

**Exemplo:**
```
GET /api/usuarios/search?nome=Carlos&page=1&pageSize=10&orderBy=nome&direction=asc
```

**Response 200:**
```json
{
  "items": [...],
  "page": 1,
  "pageSize": 10,
  "totalItems": 25,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true,
  "links": [
    { "rel": "self", "href": "/api/usuarios/search?page=1&pageSize=10", "method": "GET" },
    { "rel": "next", "href": "/api/usuarios/search?page=2&pageSize=10", "method": "GET" },
    { "rel": "last", "href": "/api/usuarios/search?page=3&pageSize=10", "method": "GET" }
  ]
}
```

---

### Check-ins

#### `GET /api/checkins`
Lista todos os check-ins.

#### `GET /api/checkins/{id}`
Busca check-in por ID.

#### `POST /api/checkins`
Cria novo check-in.

**Request Body:**
```json
{
  "usuarioId": 1,
  "nivelEstresse": 7,
  "nivelMotivacao": 6,
  "nivelCansaco": 8,
  "nivelSatisfacao": 5,
  "qualidadeSono": 4,
  "localTrabalho": "CASA",
  "observacao": "Dia cansativo, muitas reuniões"
}
```

#### `PUT /api/checkins/{id}`
Atualiza check-in existente.

#### `DELETE /api/checkins/{id}`
Remove check-in.

#### `GET /api/checkins/search`
Busca avançada de check-ins.

**Query Parameters:**
- `usuarioId` - Filtrar por ID do usuário
- `dataInicio` - Data inicial (formato: yyyy-MM-dd)
- `dataFim` - Data final (formato: yyyy-MM-dd)
- `nivelEstresseMin` - Nível mínimo de estresse
- `nivelEstresseMax` - Nível máximo de estresse
- `localTrabalho` - Filtrar por local (CASA, ESCRITORIO)
- `page`, `pageSize`, `orderBy`, `direction`

**Exemplo:**
```
GET /api/checkins/search?usuarioId=1&dataInicio=2024-11-01&nivelEstresseMin=7
```

---

### Dicas

#### `GET /api/dicas`
Lista todas as dicas.

#### `GET /api/dicas/{id}`
Busca dica por ID.

#### `POST /api/dicas`
Cria nova dica.

**Request Body:**
```json
{
  "titulo": "Pratique meditação",
  "descricao": "10 minutos diários de meditação reduzem o estresse",
  "categoria": "GESTAO_ESTRESSE",
  "condicaoAplicacao": "nivel_estresse >= 7"
}
```

#### `PUT /api/dicas/{id}`
Atualiza dica existente.

#### `DELETE /api/dicas/{id}`
Remove dica.

#### `GET /api/dicas/search`
Busca avançada de dicas.

**Query Parameters:**
- `titulo` - Filtrar por título (contém)
- `categoria` - Filtrar por categoria (contém)
- `descricao` - Filtrar por descrição (contém)
- `page`, `pageSize`, `orderBy`, `direction`

---

## 🔍 Tratamento de Erros (ProblemDetails)

A API retorna erros no formato RFC 7807 (Problem Details).

### Exemplo de Erro 404:
```json
{
  "title": "Usuário não encontrado",
  "detail": "Não existe usuário com ID 999",
  "status": 404,
  "instance": "/api/usuarios/999"
}
```

### Exemplo de Erro 400 (Validação):
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de validação",
  "status": 400,
  "errors": {
    "nome": ["Nome é obrigatório"],
    "email": ["Email inválido"]
  }
}
```

### Códigos HTTP:
- `200 OK` - Sucesso
- `201 Created` - Recurso criado
- `204 No Content` - Deleção bem-sucedida
- `400 Bad Request` - Erro de validação
- `404 Not Found` - Recurso não encontrado
- `409 Conflict` - Conflito (ex: email duplicado)
- `500 Internal Server Error` - Erro no servidor

---

## 🔗 HATEOAS (Hypermedia)

Todos os responses incluem links de navegação seguindo o padrão HATEOAS:

```json
{
  "id": 1,
  "nome": "Carlos Silva",
  "links": [
    { "rel": "self", "href": "/api/usuarios/1", "method": "GET" },
    { "rel": "update", "href": "/api/usuarios/1", "method": "PUT" },
    { "rel": "delete", "href": "/api/usuarios/1", "method": "DELETE" },
    { "rel": "checkins", "href": "/api/checkins?usuarioId=1", "method": "GET" }
  ]
}
```

---

## 💾 Banco de Dados

### Entidades:
- **GS_USUARIO** - Usuários do sistema
- **GS_CHECKIN** - Check-ins diários
- **GS_INSIGHT** - Insights identificados
- **GS_DICA** - Dicas de bem-estar
- **GS_INSIGHT_DICA** - Relacionamento N:N

### Relacionamentos:
- Usuario → Checkins (1:N)
- Usuario → Insights (1:N)
- Insight ↔ Dica (N:N via InsightDica)

---


