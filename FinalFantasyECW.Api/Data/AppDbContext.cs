using FinalFantasyECW.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalFantasyECW.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<Weapon> Weapons => Set<Weapon>();
    public DbSet<WeaponAbilityEffect> WeaponAbilityEffects => Set<WeaponAbilityEffect>();
    public DbSet<Outfit> Outfits => Set<Outfit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Code).HasMaxLength(40).IsRequired();
        });

        modelBuilder.Entity<Weapon>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.AbilityName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.AbilityDescription).HasMaxLength(500).IsRequired();
            entity.Property(x => x.SourceBanner).HasMaxLength(120);
            entity.Property(x => x.Tags).HasMaxLength(250);
            entity.Property(x => x.NormalizedSearchText).HasMaxLength(1000);
            entity.Property(x => x.StatusEffect).HasMaxLength(80);
            entity.Property(x => x.CommunityRating).HasPrecision(3, 2);

            entity.HasIndex(x => x.CharacterId);
            entity.HasIndex(x => x.Element);
            entity.HasIndex(x => x.AbilityType);
            entity.HasIndex(x => x.AbilityElement);
            entity.HasIndex(x => x.CommunityRating);
            entity.HasIndex(x => x.NormalizedSearchText);

            entity.HasOne(x => x.Character)
                .WithMany(x => x.Weapons)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WeaponAbilityEffect>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.WeaponId);
            entity.HasIndex(x => x.EffectType);
            entity.HasIndex(x => x.Tier);

            entity.HasOne(x => x.Weapon)
                .WithMany(x => x.AbilityEffects)
                .HasForeignKey(x => x.WeaponId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Outfit>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.AbilityName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.AbilityDescription).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Tags).HasMaxLength(250);

            entity.HasOne(x => x.Character)
                .WithMany(x => x.Outfits)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
