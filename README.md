# FinalFantasyECW - Web API (.NET 8 + Entity Framework Core)

API para builds de **Final Fantasy VII Ever Crisis**, baseada em **Entity Framework Core** com **SQLite**, com frontend estático para exploração rápida dos dados.

## Resumo do que a aplicação faz

A aplicação fornece uma API para:
- listar personagens;
- listar armas com filtros avançados (categoria, elemento, tipo de habilidade, tier de efeito, limites de ATB, rating, pesquisa textual, paginação, etc.);
- listar outfits (global ou por personagem);
- calcular builds com regras de equipamento (1 arma principal a 100% + até 4 secundárias a 50%).

Além da API, a rota `/` serve um frontend mock (`wwwroot/index.html`) que facilita testes rápidos de filtros sem precisar de Postman/curl.

## Arquitetura (resumo)

- **Program.cs**: bootstrap da aplicação, DI, EF Core, OpenAPI e mapeamento de controllers.
- **Controllers/EquipmentController.cs**: endpoints HTTP (`/api/characters`, `/api/weapons`, `/api/outfits`, `/api/builds/calculate`).
- **Services/EquipmentService.cs**: regras de negócio (filtros, validações e cálculo de build).
- **Data/AppDbContext.cs** + **Data/DbSeeder.cs**: acesso a dados e seed inicial.
- **Models/** e **Dtos/**: contratos da API e entidades persistidas.

## Stack

- ASP.NET Core Web API (Controllers)
- .NET 8 (`net8.0`)
- Entity Framework Core + SQLite
- OpenAPI

## Endpoints

### `GET /api/characters`
Lista personagens.

### `GET /api/weapons`
Filtros disponíveis (query string):
- `characterId`
- `category`
- `element`
- `abilityType`
- `abilityElement`
- `effectType`
- `effectTier` (1-3)
- `minPhysicalAttack`
- `minMagicalAttack`
- `minHealing`
- `minAbilityPotency`
- `minDamagePercentage`
- `maxAbilityAtbCost`
- `isLimited`
- `minCommunityRating`
- `search`
- `page`
- `pageSize`

Exemplo:
```bash
curl "http://localhost:5000/api/weapons?abilityElement=Fire&effectType=IncreasePhysicalAttack&effectTier=2&minDamagePercentage=700&page=1&pageSize=20"
```

### `GET /api/outfits`
Lista outfits, opcionalmente por personagem (`characterId`).

### `POST /api/builds/calculate`
Calcula build:
- arma principal: 100%
- secundárias (máx. 4): 50%

## Como resolver "SDK .NET não está instalado"

Neste projeto é necessário **.NET SDK 8**, porque o `TargetFramework` é `net8.0`.

### 1) Verificar se tens SDK
```bash
dotnet --info
```

Se não existir comando `dotnet` ou não aparecer `8.0.x`, instala o SDK.

### 2) Instalar o SDK .NET 8

Site oficial da Microsoft:
- https://dotnet.microsoft.com/download/dotnet/8.0

Escolhe o instalador do teu sistema operativo.

#### Ubuntu/Debian (exemplo genérico)
Segue os comandos oficiais da Microsoft para adicionar feed e instalar `dotnet-sdk-8.0`.

#### Windows
- Instalar o `.exe` do SDK.
- Reiniciar terminal/VS Code.

#### macOS
- Instalar `.pkg` oficial.
- Reiniciar terminal.

### 3) Confirmar instalação
```bash
dotnet --list-sdks
```
Deves ver uma linha `8.0.xxx`.

## Guia completo para clonar e correr na tua máquina

### Pré-requisitos
- Git
- .NET SDK 8

### Passos

1. Clonar repositório:
```bash
git clone <URL_DO_REPOSITORIO>
cd FinalFantasyECW
```

2. Restaurar pacotes:
```bash
dotnet restore
```

3. Correr API:
```bash
dotnet run --project FinalFantasyECW.Api
```

4. Abrir aplicação:
- API: URL mostrada na consola (normalmente `http://localhost:5000` ou `https://localhost:7xxx`)
- Frontend mock: mesma base URL, rota `/`

### Base de dados
- A aplicação cria/usa SQLite automaticamente.
- O seed inicial é aplicado no arranque via `DbSeeder`.

## Melhorias feitas nesta revisão
- Endpoints que estavam todos em `Program.cs` foram movidos para `Controllers/EquipmentController.cs`.
- `Program.cs` ficou mais limpo e focado em configuração e pipeline.

