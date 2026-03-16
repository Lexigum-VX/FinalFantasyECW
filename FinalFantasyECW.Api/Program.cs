using FinalFantasyECW.Api.Data;
using FinalFantasyECW.Api.Dtos;
using FinalFantasyECW.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EquipmentService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/characters", async (EquipmentService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.GetCharactersAsync(cancellationToken)))
    .WithName("GetCharacters");

app.MapGet("/api/weapons", async (
        EquipmentService service,
        Guid? characterId,
        string? category,
        string? element,
        string? abilityType,
        string? abilityElement,
        string? effectType,
        int? minPhysicalAttack,
        int? minMagicalAttack,
        int? minHealing,
        int? minAbilityPotency,
        int? minDamagePercentage,
        int? maxAbilityAtbCost,
        int? effectTier,
        bool? isLimited,
        decimal? minCommunityRating,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken) =>
    {
        var filter = new WeaponFilterRequest
        {
            CharacterId = characterId,
            Category = Enum.TryParse<FinalFantasyECW.Api.Enums.WeaponCategory>(category, true, out var c) ? c : null,
            Element = Enum.TryParse<FinalFantasyECW.Api.Enums.ElementType>(element, true, out var e) ? e : null,
            AbilityType = Enum.TryParse<FinalFantasyECW.Api.Enums.AbilityType>(abilityType, true, out var a) ? a : null,
            AbilityElement = Enum.TryParse<FinalFantasyECW.Api.Enums.ElementType>(abilityElement, true, out var ae) ? ae : null,
            EffectType = Enum.TryParse<FinalFantasyECW.Api.Enums.AbilityEffectType>(effectType, true, out var et) ? et : null,
            EffectTier = effectTier is >= 1 and <= 3 ? (FinalFantasyECW.Api.Enums.EffectTier)effectTier.Value : null,
            MinPhysicalAttack = minPhysicalAttack,
            MinMagicalAttack = minMagicalAttack,
            MinHealing = minHealing,
            MinAbilityPotency = minAbilityPotency,
            MinDamagePercentage = minDamagePercentage,
            MaxAbilityAtbCost = maxAbilityAtbCost,
            IsLimited = isLimited,
            MinCommunityRating = minCommunityRating,
            Search = search,
            Page = page == 0 ? 1 : page,
            PageSize = pageSize == 0 ? 25 : pageSize
        };

        return Results.Ok(await service.GetWeaponsAsync(filter, cancellationToken));
    })
    .WithName("GetWeapons");

app.MapGet("/api/outfits", async (EquipmentService service, Guid? characterId, CancellationToken cancellationToken) =>
    Results.Ok(await service.GetOutfitsAsync(characterId, cancellationToken)))
    .WithName("GetOutfits");

app.MapPost("/api/builds/calculate", async (EquipmentService service, BuildCalculationRequest request, CancellationToken cancellationToken) =>
    {
        try
        {
            return Results.Ok(await service.CalculateBuildAsync(request, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("CalculateBuild");

app.Run();
