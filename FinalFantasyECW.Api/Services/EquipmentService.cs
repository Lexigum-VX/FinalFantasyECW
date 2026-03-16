using FinalFantasyECW.Api.Data;
using FinalFantasyECW.Api.Dtos;
using FinalFantasyECW.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalFantasyECW.Api.Services;

public sealed class EquipmentService(AppDbContext dbContext)
{
    public async Task<IReadOnlyCollection<Character>> GetCharactersAsync(CancellationToken cancellationToken = default)
        => await dbContext.Characters.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Outfit>> GetOutfitsAsync(Guid? characterId, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Outfits.AsNoTracking().AsQueryable();

        if (characterId.HasValue)
        {
            query = query.Where(o => o.CharacterId == characterId.Value);
        }

        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Weapon>> GetWeaponsAsync(WeaponFilterRequest filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Weapons.AsNoTracking().AsQueryable();

        if (filter.CharacterId.HasValue)
            query = query.Where(w => w.CharacterId == filter.CharacterId.Value);

        if (filter.Category.HasValue)
            query = query.Where(w => w.Category == filter.Category.Value);

        if (filter.Element.HasValue)
            query = query.Where(w => w.Element == filter.Element.Value);

        if (filter.AbilityType.HasValue)
            query = query.Where(w => w.AbilityType == filter.AbilityType.Value);

        if (filter.MinPhysicalAttack.HasValue)
            query = query.Where(w => w.PhysicalAttack >= filter.MinPhysicalAttack.Value);

        if (filter.MinMagicalAttack.HasValue)
            query = query.Where(w => w.MagicalAttack >= filter.MinMagicalAttack.Value);

        if (filter.MinHealing.HasValue)
            query = query.Where(w => w.Healing >= filter.MinHealing.Value);

        if (filter.MinAbilityPotency.HasValue)
            query = query.Where(w => w.AbilityPotency >= filter.MinAbilityPotency.Value);

        if (filter.MaxAbilityAtbCost.HasValue)
            query = query.Where(w => w.AbilityAtbCost <= filter.MaxAbilityAtbCost.Value);

        if (filter.IsLimited.HasValue)
            query = query.Where(w => w.IsLimited == filter.IsLimited.Value);

        if (filter.MinCommunityRating.HasValue)
            query = query.Where(w => w.CommunityRating >= filter.MinCommunityRating.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLowerInvariant();
            query = query.Where(w => w.NormalizedSearchText.Contains(search));
        }

        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize is < 1 or > 100 ? 25 : filter.PageSize;

        return await query
            .OrderByDescending(w => w.PopularityScore)
            .ThenByDescending(w => w.CommunityRating)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<BuildCalculationResult> CalculateBuildAsync(BuildCalculationRequest request, CancellationToken cancellationToken = default)
    {
        if (request.SecondaryWeaponIds.Count > 4)
            throw new ArgumentException("Uma personagem pode equipar no máximo 5 armas (1 principal + 4 secundárias).");

        var character = await dbContext.Characters.AsNoTracking().SingleOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken)
            ?? throw new ArgumentException("CharacterId inválido.");

        var primary = await dbContext.Weapons.AsNoTracking().SingleOrDefaultAsync(w => w.Id == request.PrimaryWeaponId, cancellationToken)
            ?? throw new ArgumentException("PrimaryWeaponId inválido.");

        if (primary.CharacterId != request.CharacterId)
            throw new ArgumentException("A arma principal não pertence à personagem indicada.");

        var secondaryIds = request.SecondaryWeaponIds.Distinct().ToList();
        if (secondaryIds.Count != request.SecondaryWeaponIds.Count)
            throw new ArgumentException("As armas secundárias não podem repetir IDs.");

        if (secondaryIds.Contains(primary.Id))
            throw new ArgumentException("A arma principal não pode estar também nas secundárias.");

        var secondaries = await dbContext.Weapons.AsNoTracking()
            .Where(w => secondaryIds.Contains(w.Id))
            .ToListAsync(cancellationToken);

        if (secondaries.Count != secondaryIds.Count)
            throw new ArgumentException("Uma ou mais armas secundárias não existem.");

        if (secondaries.Any(w => w.CharacterId != request.CharacterId))
            throw new ArgumentException("Todas as armas secundárias têm de pertencer à personagem indicada.");

        var totalPhysical = primary.PhysicalAttack + secondaries.Sum(w => w.PhysicalAttack * 0.5m);
        var totalMagical = primary.MagicalAttack + secondaries.Sum(w => w.MagicalAttack * 0.5m);
        var totalHealing = primary.Healing + secondaries.Sum(w => w.Healing * 0.5m);

        var activeAbilities = new List<string> { primary.AbilityName };
        activeAbilities.AddRange(secondaries.Select(w => w.AbilityName));

        return new BuildCalculationResult(
            request.CharacterId,
            character.Name,
            primary.Id,
            secondaryIds,
            totalPhysical,
            totalMagical,
            totalHealing,
            activeAbilities);
    }
}
