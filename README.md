# CodeFlix
#### Admin do Catalogo de videos

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=luiszkm_CodeFlix.Catalog&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=luiszkm_CodeFlix.Catalog)

## Features

- Cadastro e gerenciamento de videos.
- Filtrar e ordenar o video por genero e categorias.
- Cadatrar, editar e deletar genero e categorias.
- Membros do elenco relacionado as videos paraticipantes
- Stramer do Video

## Pré-requisitos

- Docker: [Instalação](https://docs.docker.com/get-docker/)
- Docker Compose: [Instalação](https://docs.docker.com/compose/install/)

## Instalação

1. Clone este repositório:
```bash
git clone https://github.com/luiszkm/CodeFlix.Catalog.git
```
2. Configure as variáveis de ambiente:
Renomeie o arquivo .env.example para .env e preencha as variáveis de acordo com as suas configurações.

3. Execute o comando:
```bash
docker compose up
```


## Tech Stack



**Server:** C# | .Net | ASP.NET Core | MySQL

**Libs:** MediatR | EF Core | Fluent Validation |
Polly | RabitMq | JWT

**Padrões de projetos:** Clean Arquiteture |
DDD | Clean Code

**CI/CD:** Github Actions | Docker

**Ferramentas:** Docker | GCP Clound Storage



