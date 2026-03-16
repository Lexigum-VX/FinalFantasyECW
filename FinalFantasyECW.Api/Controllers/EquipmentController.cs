using FinalFantasyECW.Api.Dtos;
using FinalFantasyECW.Api.Enums;
using FinalFantasyECW.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalFantasyECW.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class EquipmentController(EquipmentService service) : ControllerBase
{
    [HttpGet("characters")]
    public async Task<IActionResult> GetCharacters(CancellationToken cancellationToken)
        => Ok(await service.GetCharactersAsync(cancellationToken));

    [HttpGet("weapons")]
    public async Task<IActionResult> GetWeapons(
        [FromQuery] Guid? characterId,
        [FromQuery] string? category,
        [FromQuery] string? element,
        [FromQuery] string? abilityType,
        [FromQuery] string? abilityElement,
        [FromQuery] string? effectType,
        [FromQuery] int? minPhysicalAttack,
        [FromQuery] int? minMagicalAttack,
        [FromQuery] int? minHealing,
        [FromQuery] int? minAbilityPotency,
        [FromQuery] int? minDamagePercentage,
        [FromQuery] int? maxAbilityAtbCost,
        [FromQuery] int? effectTier,
        [FromQuery] bool? isLimited,
        [FromQuery] decimal? minCommunityRating,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var filter = new WeaponFilterRequest
        {
            CharacterId = characterId,
            Category = Enum.TryParse<WeaponCategory>(category, true, out var c) ? c : null,
            Element = Enum.TryParse<ElementType>(element, true, out var e) ? e : null,
            AbilityType = Enum.TryParse<AbilityType>(abilityType, true, out var a) ? a : null,
            AbilityElement = Enum.TryParse<ElementType>(abilityElement, true, out var ae) ? ae : null,
            EffectType = Enum.TryParse<AbilityEffectType>(effectType, true, out var et) ? et : null,
            EffectTier = effectTier is >= 1 and <= 3 ? (EffectTier)effectTier.Value : null,
            MinPhysicalAttack = minPhysicalAttack,
            MinMagicalAttack = minMagicalAttack,
            MinHealing = minHealing,
            MinAbilityPotency = minAbilityPotency,
            MinDamagePercentage = minDamagePercentage,
            MaxAbilityAtbCost = maxAbilityAtbCost,
            IsLimited = isLimited,
            MinCommunityRating = minCommunityRating,
            Search = search,
            Page = page,
            PageSize = pageSize
        };

        return Ok(await service.GetWeaponsAsync(filter, cancellationToken));
    }

    [HttpGet("outfits")]
    public async Task<IActionResult> GetOutfits([FromQuery] Guid? characterId, CancellationToken cancellationToken)
        => Ok(await service.GetOutfitsAsync(characterId, cancellationToken));

    [HttpPost("builds/calculate")]
    public async Task<IActionResult> CalculateBuild([FromBody] BuildCalculationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await service.CalculateBuildAsync(request, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
