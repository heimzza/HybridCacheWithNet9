using FormulaOne.Api.Services;
using FormulaOne.DataService.Persistence;
using FormulaOne.Entities.DbSet;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DriversController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;
    
    public DriversController(IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }
    
    [HttpGet]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> GetDriver(Guid driverId)
    {
        var cacheKey = $"Driver_{driverId}";

        var cachedDriver = _cachingService.Get<Driver>(cacheKey);

        if (cachedDriver is not null)
        {
            return Ok(cachedDriver);
        }

        return NotFound($"No driver found with id {driverId}");
    }

    [HttpPost]
    public async Task<IActionResult> UpsertDriver(Driver driver)
    {
        if (driver is null)
        {
            return BadRequest();
        }
        
        await _unitOfWork.Drivers.AddAsync(driver);

        await _unitOfWork.SaveChangesAsync();
        
        return Ok(driver);
    }
}