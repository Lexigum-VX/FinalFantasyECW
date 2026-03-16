using FinalFantasyECW.Api.Enums;
using FinalFantasyECW.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalFantasyECW.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (await dbContext.Characters.AnyAsync(cancellationToken))
        {
            return;
        }

        var characters = new List<Character>
        {
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Cloud", Code = "cloud" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Barret", Code = "barret" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Tifa", Code = "tifa" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Aerith", Code = "aerith" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = "Red XIII", Code = "red-xiii" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = "Zack", Code = "zack" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Name = "Sephiroth", Code = "sephiroth" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Name = "Glenn", Code = "glenn" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Name = "Matt", Code = "matt" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Name = "Lucia", Code = "lucia" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), Name = "Young Sephiroth", Code = "young-sephiroth" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000012"), Name = "Young Angeal", Code = "young-angeal" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000013"), Name = "Young Genesis", Code = "young-genesis" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000014"), Name = "Yuffie", Code = "yuffie" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000015"), Name = "Cait Sith", Code = "cait-sith" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000016"), Name = "Vincent", Code = "vincent" }
        };

        var abilities = new[] { "Brave Slash", "Arcane Burst", "Curaga Pulse", "Armor Break", "Magic Break", "Regen Veil" };
        var elements = new[] { ElementType.Fire, ElementType.Ice, ElementType.Lightning, ElementType.Earth, ElementType.Wind, ElementType.None };
        var weapons = new List<Weapon>();
        var outfits = new List<Outfit>();

        foreach (var (character, index) in characters.Select((c, i) => (c, i)))
        {
            var baseRelease = new DateOnly(2024, 1, 1).AddDays(index * 7);

            weapons.Add(CreateWeapon(character, index, "A", WeaponCategory.Sword, abilities[index % abilities.Length], elements[index % elements.Length], AbilityType.PhysicalDamage, 320 + (index * 8), 130 + (index * 4), 70 + index, 300 + index * 5, 20 + index, 20 + index, 700 + index * 15, 3, false, baseRelease, "Launch", 760));
            weapons.Add(CreateWeapon(character, index, "B", WeaponCategory.Staff, abilities[(index + 2) % abilities.Length], elements[(index + 2) % elements.Length], AbilityType.MagicalDamage, 180 + (index * 4), 360 + (index * 8), 90 + index, 280 + index * 4, 18 + index, 24 + index, 760 + index * 14, 4, index % 2 == 0, baseRelease.AddDays(10), "Event Banner", 820));
            weapons.Add(CreateWeapon(character, index, "C", WeaponCategory.Gun, abilities[(index + 4) % abilities.Length], elements[(index + 4) % elements.Length], AbilityType.Heal, 140 + (index * 3), 150 + (index * 4), 330 + (index * 5), 260 + index * 5, 16 + index, 26 + index, 640 + index * 16, 4, index % 3 == 0, baseRelease.AddDays(20), "Seasonal Banner", 0));

            outfits.Add(new Outfit
            {
                Id = Guid.Parse($"10000000-0000-0000-0000-{(index + 1).ToString("D12")}"),
                CharacterId = character.Id,
                Name = $"{character.Name} Outfit",
                AbilityName = $"Passive {(index % 6) + 1}",
                AbilityDescription = "Bónus passivo para build especializada.",
                Tags = "starter,utility"
            });
        }

        dbContext.Characters.AddRange(characters);
        dbContext.Weapons.AddRange(weapons);
        dbContext.Outfits.AddRange(outfits);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Weapon CreateWeapon(
        Character character,
        int index,
        string suffix,
        WeaponCategory category,
        string abilityName,
        ElementType element,
        AbilityType abilityType,
        int physicalAttack,
        int magicalAttack,
        int healing,
        int hp,
        int pDef,
        int mDef,
        int potency,
        int atbCost,
        bool isLimited,
        DateOnly releaseDate,
        string source,
        int damagePercentage)
    {
        var name = $"{character.Name} Weapon {suffix}";
        var effects = BuildEffects(index, suffix);
        var effectTags = string.Join(',', effects.Select(x => x.EffectType.ToString().ToLowerInvariant()));
        var tags = $"{character.Code},{abilityType.ToString().ToLowerInvariant()},{element.ToString().ToLowerInvariant()},{effectTags}";
        var normalizedSearch = $"{name} {abilityName} {tags}".ToLowerInvariant();

        var weaponId = Guid.Parse($"20000000-0000-0000-0000-{(index * 3 + (suffix[0] - 'A') + 1).ToString("D12")}");
        effects.ForEach(effect => effect.WeaponId = weaponId);

        return new Weapon
        {
            Id = weaponId,
            Name = name,
            CharacterId = character.Id,
            Category = category,
            Rarity = 5,
            OverboostLevel = 0,
            Element = element,
            PhysicalAttack = physicalAttack,
            MagicalAttack = magicalAttack,
            Healing = healing,
            Hp = hp,
            PhysicalDefense = pDef,
            MagicalDefense = mDef,
            AbilityName = abilityName,
            AbilityDescription = "Habilidade principal da arma para dano, cura ou suporte.",
            AbilityType = abilityType,
            AbilityTarget = abilityType == AbilityType.Heal ? AbilityTarget.SingleAlly : AbilityTarget.SingleEnemy,
            AbilityElement = element,
            DamagePercentage = damagePercentage,
            AbilityPotency = potency,
            AbilityAtbCost = atbCost,
            AbilityDurationSeconds = abilityType is AbilityType.Buff or AbilityType.Debuff ? 12 : 0,
            StatusEffect = abilityType is AbilityType.Buff or AbilityType.Debuff ? "Buff/Debuff" : null,
            AbilityEffects = effects,
            IsLimited = isLimited,
            ReleaseDate = releaseDate,
            SourceBanner = source,
            Tags = tags,
            NormalizedSearchText = normalizedSearch,
            CommunityRating = 3.5m + (index % 3) * 0.3m,
            PopularityScore = 50 + (index * 3)
        };
    }

    private static List<WeaponAbilityEffect> BuildEffects(int index, string suffix)
    {
        var suffixOffset = suffix[0] - 'A';
        var firstTier = (EffectTier)(((index + suffixOffset) % 3) + 1);
        var secondTier = (EffectTier)(((index + suffixOffset + 1) % 3) + 1);

        return
        [
            new()
            {
                Id = Guid.NewGuid(),
                EffectType = AbilityEffectType.IncreasePhysicalAttack,
                Tier = firstTier,
                ValuePercentage = firstTier switch { EffectTier.Tier1 => 10, EffectTier.Tier2 => 20, _ => 30 }
            },
            new()
            {
                Id = Guid.NewGuid(),
                EffectType = index % 2 == 0 ? AbilityEffectType.DecreaseDefense : AbilityEffectType.IncreaseDefense,
                Tier = secondTier,
                ValuePercentage = secondTier switch { EffectTier.Tier1 => 10, EffectTier.Tier2 => 20, _ => 30 }
            },
            new()
            {
                Id = Guid.NewGuid(),
                EffectType = AbilityEffectType.IncreaseMagicalAttack,
                Tier = (EffectTier)(((index + suffixOffset + 2) % 3) + 1),
                ValuePercentage = 15
            }
        ];
    }
}
