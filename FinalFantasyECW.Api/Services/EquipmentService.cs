using FinalFantasyECW.Api.Data;
using FinalFantasyECW.Api.Models;

namespace FinalFantasyECW.Api.Services;

public sealed class EquipmentService
{
    public IReadOnlyCollection<Character> GetCharacters() => GameData.Characters;

    public IReadOnlyCollection<Outfit> GetOutfits(Guid? characterId = null)
    {
        var query = GameData.Outfits.AsEnumerable();

        if (characterId.HasValue)
        {
            query = query.Where(o => o.CharacterId == characterId.Value);
        }

        return query.ToList();
    }

    public IReadOnlyCollection<Weapon> GetWeapons(WeaponFilter filter)
    {
        var query = GameData.Weapons.AsEnumerable();

        if (filter.CharacterId.HasValue)
        {
            query = query.Where(w => w.CharacterId == filter.CharacterId.Value);
        }

        if (filter.MinPhysicalAttack.HasValue)
        {
            query = query.Where(w => w.PhysicalAttack >= filter.MinPhysicalAttack.Value);
        }

        if (filter.MinMagicalAttack.HasValue)
        {
            query = query.Where(w => w.MagicalAttack >= filter.MinMagicalAttack.Value);
        }

        if (filter.MinHealing.HasValue)
        {
            query = query.Where(w => w.Healing >= filter.MinHealing.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.AbilityContains))
        {
            query = query.Where(w => w.Ability.Contains(filter.AbilityContains.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        return query
            .OrderByDescending(w => w.PhysicalAttack + w.MagicalAttack + w.Healing)
            .ToList();
    }

    public BuildCalculationResult CalculateBuild(BuildCalculationRequest request)
    {
        if (request.SecondaryWeaponIds.Count > 4)
        {
            throw new ArgumentException("Uma personagem pode equipar no máximo 5 armas (1 principal + 4 secundárias).");
        }

        var character = GameData.Characters.SingleOrDefault(c => c.Id == request.CharacterId)
            ?? throw new ArgumentException("CharacterId inválido.");

        var primary = GameData.Weapons.SingleOrDefault(w => w.Id == request.PrimaryWeaponId)
            ?? throw new ArgumentException("PrimaryWeaponId inválido.");

        if (primary.CharacterId != request.CharacterId)
        {
            throw new ArgumentException("A arma principal não pertence à personagem indicada.");
        }

        var secondaryIds = request.SecondaryWeaponIds.Distinct().ToList();
        if (secondaryIds.Count != request.SecondaryWeaponIds.Count)
        {
            throw new ArgumentException("As armas secundárias não podem repetir IDs.");
        }

        var secondaries = GameData.Weapons.Where(w => secondaryIds.Contains(w.Id)).ToList();
        if (secondaries.Count != secondaryIds.Count)
        {
            throw new ArgumentException("Uma ou mais armas secundárias não existem.");
        }

        if (secondaries.Any(w => w.CharacterId != request.CharacterId))
        {
            throw new ArgumentException("Todas as armas secundárias têm de pertencer à personagem indicada.");
        }

        if (secondaryIds.Contains(primary.Id))
        {
            throw new ArgumentException("A arma principal não pode estar também nas secundárias.");
        }

        var totalPhysical = primary.PhysicalAttack + secondaries.Sum(w => w.PhysicalAttack * 0.5m);
        var totalMagical = primary.MagicalAttack + secondaries.Sum(w => w.MagicalAttack * 0.5m);
        var totalHealing = primary.Healing + secondaries.Sum(w => w.Healing * 0.5m);

        var abilities = new List<string> { primary.Ability };
        abilities.AddRange(secondaries.Select(w => w.Ability));

        return new BuildCalculationResult(
            request.CharacterId,
            character.Name,
            primary.Id,
            secondaryIds,
            totalPhysical,
            totalMagical,
            totalHealing,
            abilities);
    }
}
