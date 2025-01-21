using FormulaOne.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public class DriversController : Controller
{
    private readonly ICachingService _cachingService;
    
    public DriversController(ICachingService cachingService)
    {
        _cachingService = cachingService;
    }
    
    [HttpGet]
    [Route("{driverId:guid}")]
    public Task<IActionResult> GetDriver(Guid driverId)
    {
        var cacheKey = $"Driver_{driverId}";

        var cachedDriver = _cachingService.Get<Driver>(cacheKey);

        if (cachedDriver is not null)
        {
            return Ok(cachedDriver);
        }
    }
}