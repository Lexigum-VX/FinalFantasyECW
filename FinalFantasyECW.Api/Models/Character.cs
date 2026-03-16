namespace FinalFantasyECW.Api.Models;

public sealed class Character
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
    public ICollection<Outfit> Outfits { get; set; } = new List<Outfit>();
}
