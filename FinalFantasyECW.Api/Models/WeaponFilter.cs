namespace FinalFantasyECW.Api.Models;

public sealed class WeaponFilter
{
    public Guid? CharacterId { get; init; }
    public int? MinPhysicalAttack { get; init; }
    public int? MinMagicalAttack { get; init; }
    public int? MinHealing { get; init; }
    public string? AbilityContains { get; init; }
}
