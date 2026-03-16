using FinalFantasyECW.Api.Models;

namespace FinalFantasyECW.Api.Data;

public static class GameData
{
    public static IReadOnlyCollection<Character> Characters { get; } = new List<Character>
    {
        new(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Cloud"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Barret"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Tifa"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Aerith"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000005"), "Red XIII"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000006"), "Zack"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000007"), "Sephiroth"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000008"), "Glenn"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000009"), "Matt"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000010"), "Lucia"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000011"), "Young Sephiroth"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000012"), "Young Angeal"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000013"), "Young Genesis"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000014"), "Yuffie"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000015"), "Cait Sith"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000016"), "Vincent")
    };

    public static IReadOnlyCollection<Weapon> Weapons { get; } = BuildWeapons();
    public static IReadOnlyCollection<Outfit> Outfits { get; } = BuildOutfits();

    private static IReadOnlyCollection<Weapon> BuildWeapons()
    {
        var abilities = new[] { "Brave Slash", "Arcane Burst", "Curaga Pulse", "Armor Break", "Magic Break", "Regen Veil" };
        var weapons = new List<Weapon>();

        foreach (var (character, index) in Characters.Select((c, i) => (c, i)))
        {
            weapons.Add(new Weapon(
                Guid.NewGuid(),
                $"{character.Name} Weapon A",
                character.Id,
                130 + (index * 6),
                70 + (index * 4),
                45 + (index * 3),
                abilities[index % abilities.Length]));

            weapons.Add(new Weapon(
                Guid.NewGuid(),
                $"{character.Name} Weapon B",
                character.Id,
                90 + (index * 5),
                120 + (index * 6),
                60 + (index * 2),
                abilities[(index + 2) % abilities.Length]));

            weapons.Add(new Weapon(
                Guid.NewGuid(),
                $"{character.Name} Weapon C",
                character.Id,
                80 + (index * 3),
                65 + (index * 2),
                130 + (index * 5),
                abilities[(index + 4) % abilities.Length]));
        }

        return weapons;
    }

    private static IReadOnlyCollection<Outfit> BuildOutfits()
    {
        var abilities = new[] { "ATB Start", "Limit Charge", "Party Regen", "PDEF Up", "MDEF Up", "Debuff Resist" };
        return Characters
            .Select((character, index) => new Outfit(
                Guid.NewGuid(),
                $"{character.Name} Outfit",
                character.Id,
                abilities[index % abilities.Length]))
            .ToList();
    }
}
