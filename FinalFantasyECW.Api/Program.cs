using FinalFantasyECW.Api.Models;
using FinalFantasyECW.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<EquipmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/characters", (EquipmentService service) => Results.Ok(service.GetCharacters()))
    .WithName("GetCharacters");

app.MapGet("/api/weapons", (
        EquipmentService service,
        Guid? characterId,
        int? minPhysicalAttack,
        int? minMagicalAttack,
        int? minHealing,
        string? abilityContains) =>
    {
        var filter = new WeaponFilter
        {
            CharacterId = characterId,
            MinPhysicalAttack = minPhysicalAttack,
            MinMagicalAttack = minMagicalAttack,
            MinHealing = minHealing,
            AbilityContains = abilityContains
        };

        return Results.Ok(service.GetWeapons(filter));
    })
    .WithName("GetWeapons");

app.MapGet("/api/outfits", (EquipmentService service, Guid? characterId) =>
    Results.Ok(service.GetOutfits(characterId)))
    .WithName("GetOutfits");

app.MapPost("/api/builds/calculate", (EquipmentService service, BuildCalculationRequest request) =>
    {
        try
        {
            return Results.Ok(service.CalculateBuild(request));
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    })
    .WithName("CalculateBuild");

app.Run();
