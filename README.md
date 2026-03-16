# FinalFantasyECW - Web API (.NET 10)

API inicial para gerir personagens, armas e fatos, com foco em **filtros de armas** e **cálculo de build**.

## Stack
- ASP.NET Core Minimal API
- .NET 10 (`net10.0`)
- Dados em memória (seed inicial para 16 personagens)

## Endpoints

### `GET /api/characters`
Lista as 16 personagens.

### `GET /api/weapons`
Filtra armas por atributos e habilidade.

Query params opcionais:
- `characterId`
- `minPhysicalAttack`
- `minMagicalAttack`
- `minHealing`
- `abilityContains`

Exemplo:

```bash
curl "http://localhost:5000/api/weapons?minPhysicalAttack=140&abilityContains=Break"
```

### `GET /api/outfits`
Lista fatos. Pode filtrar por personagem:

```bash
curl "http://localhost:5000/api/outfits?characterId=<GUID>"
```

### `POST /api/builds/calculate`
Calcula a build de uma personagem com a regra:
- Arma principal: 100% atributos
- Armas secundárias (máximo 4): 50% atributos cada

Payload exemplo:

```json
{
  "characterId": "00000000-0000-0000-0000-000000000001",
  "primaryWeaponId": "<weapon-guid>",
  "secondaryWeaponIds": ["<weapon-guid-2>", "<weapon-guid-3>"]
}
```

## Próximos passos
- Persistência em base de dados (PostgreSQL/SQL Server + EF Core)
- Autenticação + registo de inventário do utilizador
- Frontend (Blazor/React/Vue) para filtro visual e comparação de builds

## Notas
Neste ambiente não foi possível correr `dotnet` porque o SDK não está instalado.
