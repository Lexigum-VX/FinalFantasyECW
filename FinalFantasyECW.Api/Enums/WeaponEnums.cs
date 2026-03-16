namespace FinalFantasyECW.Api.Enums;

public enum WeaponCategory
{
    Sword,
    Greatsword,
    Gun,
    Gloves,
    Staff,
    Spear,
    Katana,
    Dagger,
    Shuriken,
    Megaphone,
    Other
}

public enum ElementType
{
    None,
    Fire,
    Ice,
    Lightning,
    Earth,
    Wind,
    Water,
    Holy,
    Dark
}

public enum AbilityType
{
    PhysicalDamage,
    MagicalDamage,
    Heal,
    Buff,
    Debuff,
    Utility
}

public enum AbilityTarget
{
    SingleEnemy,
    AllEnemies,
    SingleAlly,
    AllAllies,
    Self
}
