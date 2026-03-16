namespace FinalFantasyECW.Api.Models;

public sealed class BuildCalculationRequest
{
    public Guid CharacterId { get; init; }
    public Guid PrimaryWeaponId { get; init; }
    public IReadOnlyCollection<Guid> SecondaryWeaponIds { get; init; } = Array.Empty<Guid>();
}
