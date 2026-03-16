namespace FinalFantasyECW.Api.Models;

public sealed record Outfit(
    Guid Id,
    string Name,
    Guid CharacterId,
    string Ability);
