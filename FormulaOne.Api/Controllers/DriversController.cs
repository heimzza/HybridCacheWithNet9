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
    public async Task<IActionResult> GetOne(Guid driverId)
    {
        var cacheKey = $"Driver_{driverId}";
        
        var cachedDriver = _cachingService.Get<Driver>(cacheKey);

        if (cachedDriver is not null)
        {
            return Ok(cachedDriver);
        }
        
        var driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
        
        if (driver is not null)
        {
            _cachingService.Set(cacheKey, driver, TimeSpan.FromMinutes(5));
            
            return Ok(driver);
        }

        return NotFound($"No driver found with id {driverId}");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        const string cacheKey = $"Drivers";
        
        var drivers = _cachingService.Get<IEnumerable<Driver>>(cacheKey);

        if (drivers is not null)
        {
            return Ok(drivers);
        }
        
        drivers = await _unitOfWork.Drivers.GetAllAsync();

        if (drivers is not null)
        {
            _cachingService.Set(cacheKey, drivers, TimeSpan.FromMinutes(5));
            
            return Ok(drivers);
        }

        return NotFound($"No driver found");
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Driver driver)
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