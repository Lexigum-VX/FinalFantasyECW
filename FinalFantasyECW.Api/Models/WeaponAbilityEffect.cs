using FinalFantasyECW.Api.Enums;

namespace FinalFantasyECW.Api.Models;

public sealed class WeaponAbilityEffect
{
    public Guid Id { get; set; }
    public Guid WeaponId { get; set; }
    public Weapon? Weapon { get; set; }

    public AbilityEffectType EffectType { get; set; }
    public EffectTier Tier { get; set; }
    public int ValuePercentage { get; set; }
}
