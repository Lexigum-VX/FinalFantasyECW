# FinalFantasyECW - Web API (.NET 10 + Entity Framework Core)

API para builds de **Final Fantasy VII Ever Crisis**, agora baseada em **Entity Framework Core** com **SQLite** para que todos os dados venham da base de dados.

## O que foi melhorado
- Persistência com EF Core (`AppDbContext`)
- Seed inicial na base de dados (`DbSeeder`) para 16 personagens
- Modelo de arma expandido com mais propriedades para pesquisa avançada
- Filtros avançados para otimizar criação de builds
- Cálculo de build com regra de equipamento (1 principal 100% + até 4 secundárias 50%)

## Stack
- ASP.NET Core Minimal API
- .NET 10 (`net10.0`)
- Entity Framework Core + SQLite

## Propriedades da arma (v1)
Inclui propriedades do jogo e extras de pesquisa:
- Base: `PhysicalAttack`, `MagicalAttack`, `Healing`, `Hp`, `PhysicalDefense`, `MagicalDefense`
- Meta: `Category`, `Element`, `Rarity`, `OverboostLevel`
- Habilidade: `AbilityName`, `AbilityDescription`, `AbilityType`, `AbilityTarget`, `AbilityElement`, `AbilityPotency`, `AbilityAtbCost`, `AbilityDurationSeconds`, `StatusEffect`
- Pesquisa avançada: `IsLimited`, `ReleaseDate`, `SourceBanner`, `Tags`, `NormalizedSearchText`, `CommunityRating`, `PopularityScore`

## Endpoints

### `GET /api/characters`
Lista personagens.

### `GET /api/weapons`
Filtros disponíveis (query string):
- `characterId`
- `category`
- `element`
- `abilityType`
- `minPhysicalAttack`
- `minMagicalAttack`
- `minHealing`
- `minAbilityPotency`
- `maxAbilityAtbCost`
- `isLimited`
- `minCommunityRating`
- `search`
- `page`
- `pageSize`

Exemplo:
```bash
curl "http://localhost:5000/api/weapons?element=Fire&abilityType=PhysicalDamage&minPhysicalAttack=140&search=break&page=1&pageSize=20"
```

### `GET /api/outfits`
Lista fatos, opcionalmente por personagem (`characterId`).

### `POST /api/builds/calculate`
Calcula build:
- Arma principal: 100%
- Secundárias (máx. 4): 50%

Payload:
```json
{
  "characterId": "00000000-0000-0000-0000-000000000001",
  "primaryWeaponId": "20000000-0000-0000-0000-000000000001",
  "secondaryWeaponIds": [
    "20000000-0000-0000-0000-000000000002",
    "20000000-0000-0000-0000-000000000003"
  ]
}
```

## Planeamento (roadmap)

### Fase 1 (atual)
- API + EF Core + SQLite
- Modelo completo inicial de personagens, armas e fatos
- Filtros e cálculo de build

### Fase 2
- Criar migrações EF e scripts de deploy (`dotnet ef migrations`)
- Trocar SQLite por PostgreSQL/SQL Server
- Índices avançados (FTS para pesquisa textual)

### Fase 3
- Autenticação (JWT)
- Tabelas de utilizador/inventário (`UserWeapons`, níveis de overboost reais, favoritos)
- Endpoint de recomendações de build por objetivo (DPS, cura, debuff)

### Fase 4
- Frontend (React/Blazor)
- Builder visual drag-and-drop
- Partilha de builds por link

## Execução local
```bash
dotnet restore
dotnet run --project FinalFantasyECW.Api
```

A aplicação cria automaticamente a base de dados SQLite e faz seed na primeira execução.

## Notas de ambiente deste workspace
Neste ambiente não foi possível correr `dotnet` porque o SDK não está instalado.
