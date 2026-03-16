# FinalFantasyECW - Web API (.NET 10 + Entity Framework Core)

API para builds de **Final Fantasy VII Ever Crisis**, agora baseada em **Entity Framework Core** com **SQLite** para que todos os dados venham da base de dados.

## O que foi melhorado
- Persistência com EF Core (`AppDbContext`)
- Seed inicial na base de dados (`DbSeeder`) para 16 personagens
- Modelo de arma expandido com suporte a **% de dano**, **elemento da habilidade** e **múltiplos efeitos por habilidade com tiers**
- Filtros avançados para otimizar criação de builds
- Cálculo de build com regra de equipamento (1 principal 100% + até 4 secundárias 50%)
- Frontend mock em `/` para visualizar rapidamente as armas e filtros

## Stack
- ASP.NET Core Minimal API
- .NET 10 (`net10.0`)
- Entity Framework Core + SQLite

## Propriedades da arma (v2)
Inclui propriedades do jogo e extras de pesquisa:
- Base: `PhysicalAttack`, `MagicalAttack`, `Healing`, `Hp`, `PhysicalDefense`, `MagicalDefense`
- Meta: `Category`, `Element`, `Rarity`, `OverboostLevel`
- Habilidade: `AbilityName`, `AbilityDescription`, `AbilityType`, `AbilityTarget`, `AbilityElement`, `DamagePercentage`, `AbilityPotency`, `AbilityAtbCost`, `AbilityDurationSeconds`, `StatusEffect`
- Efeitos da habilidade (`WeaponAbilityEffect`): `EffectType`, `Tier` (`Tier1-3`), `ValuePercentage`
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
Lista fatos, opcionalmente por personagem (`characterId`).

### `POST /api/builds/calculate`
Calcula build:
- Arma principal: 100%
- Secundárias (máx. 4): 50%

## Frontend mock
A rota `/` serve um frontend simples (`wwwroot/index.html`) com dados mock (`wwwroot/mock/weapons.json`) para testar rapidamente:
- filtro por elemento da habilidade
- filtro por tipo de efeito
- filtro por tier mínimo
- filtro por % mínima de dano

## Execução local
```bash
dotnet restore
dotnet run --project FinalFantasyECW.Api
```

A aplicação cria automaticamente a base de dados SQLite e faz seed na primeira execução.

## Notas de ambiente deste workspace
Neste ambiente não foi possível correr `dotnet` porque o SDK não está instalado.
