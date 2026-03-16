namespace FinalFantasyECW.Api.Dtos;

public sealed record BuildCalculationResult(
    Guid CharacterId,
    string CharacterName,
    Guid PrimaryWeaponId,
    IReadOnlyCollection<Guid> SecondaryWeaponIds,
    decimal TotalPhysicalAttack,
    decimal TotalMagicalAttack,
    decimal TotalHealing,
    IReadOnlyCollection<string> ActiveAbilities);
