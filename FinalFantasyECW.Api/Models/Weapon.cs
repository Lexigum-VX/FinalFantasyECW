namespace FinalFantasyECW.Api.Models;

public sealed record Weapon(
    Guid Id,
    string Name,
    Guid CharacterId,
    int PhysicalAttack,
    int MagicalAttack,
    int Healing,
    string Ability);
