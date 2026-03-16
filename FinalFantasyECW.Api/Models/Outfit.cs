namespace FinalFantasyECW.Api.Models;

public sealed class Outfit
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character? Character { get; set; }

    public string Name { get; set; } = string.Empty;
    public string AbilityName { get; set; } = string.Empty;
    public string AbilityDescription { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
}
