using FinalFantasyECW.Api.Enums;

namespace FinalFantasyECW.Api.Dtos;

public sealed class WeaponFilterRequest
{
    public Guid? CharacterId { get; init; }
    public WeaponCategory? Category { get; init; }
    public ElementType? Element { get; init; }
    public AbilityType? AbilityType { get; init; }
    public ElementType? AbilityElement { get; init; }

    public int? MinPhysicalAttack { get; init; }
    public int? MinMagicalAttack { get; init; }
    public int? MinHealing { get; init; }
    public int? MinAbilityPotency { get; init; }
    public int? MinDamagePercentage { get; init; }
    public int? MaxAbilityAtbCost { get; init; }

    public AbilityEffectType? EffectType { get; init; }
    public EffectTier? EffectTier { get; init; }

    public bool? IsLimited { get; init; }
    public decimal? MinCommunityRating { get; init; }
    public string? Search { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;
}
