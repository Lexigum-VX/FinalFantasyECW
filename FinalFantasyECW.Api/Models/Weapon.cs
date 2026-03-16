using FinalFantasyECW.Api.Enums;

namespace FinalFantasyECW.Api.Models;

public sealed class Weapon
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CharacterId { get; set; }
    public Character? Character { get; set; }

    // Propriedades base do jogo
    public WeaponCategory Category { get; set; }
    public int Rarity { get; set; }
    public int OverboostLevel { get; set; }
    public ElementType Element { get; set; }

    public int PhysicalAttack { get; set; }
    public int MagicalAttack { get; set; }
    public int Healing { get; set; }
    public int Hp { get; set; }
    public int PhysicalDefense { get; set; }
    public int MagicalDefense { get; set; }

    // Habilidade da arma
    public string AbilityName { get; set; } = string.Empty;
    public string AbilityDescription { get; set; } = string.Empty;
    public AbilityType AbilityType { get; set; }
    public AbilityTarget AbilityTarget { get; set; }
    public ElementType AbilityElement { get; set; }
    public int DamagePercentage { get; set; }
    public int AbilityPotency { get; set; }
    public int AbilityAtbCost { get; set; }
    public int AbilityDurationSeconds { get; set; }
    public string? StatusEffect { get; set; }

    public ICollection<WeaponAbilityEffect> AbilityEffects { get; set; } = new List<WeaponAbilityEffect>();

    // Propriedades adicionais para pesquisa avançada
    public bool IsLimited { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public string SourceBanner { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string NormalizedSearchText { get; set; } = string.Empty;
    public decimal CommunityRating { get; set; }
    public int PopularityScore { get; set; }
}
