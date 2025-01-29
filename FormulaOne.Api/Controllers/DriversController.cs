using FormulaOne.Api.Services;
using FormulaOne.DataService.Persistence;
using FormulaOne.Entities.DbSet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace FormulaOne.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DriversController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;
    private readonly HybridCache _hybridCache;
    public DriversController(IUnitOfWork unitOfWork, ICachingService cachingService, HybridCache hybridCache)
    {
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
        _hybridCache = hybridCache;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetOne(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Driver_{id}";

        var cachedDriver = await _hybridCache.GetOrCreateAsync(cacheKey, async t =>
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);

            return driver;
        },
            tags:["driver"],
            cancellationToken: cancellationToken
        );

        if (cachedDriver is null)
            return NotFound($"No driver found with id {id}");

        return Ok(cachedDriver);
    }
    
    // [HttpGet]
    // [Route("{id:guid}")]
    // public async Task<IActionResult> GetOne(Guid id)
    // {
    //     var cacheKey = $"Driver_{id}";
    //     
    //     var cachedDriver = _cachingService.Get<Driver>(cacheKey);
    //
    //     if (cachedDriver is not null)
    //     {
    //         return Ok(cachedDriver);
    //     }
    //     
    //     var driver = await _unitOfWork.Drivers.GetByIdAsync(id);
    //     
    //     if (driver is not null)
    //     {
    //         _cachingService.Set(cacheKey, driver, TimeSpan.FromMinutes(5));
    //         
    //         return Ok(driver);
    //     }
    //
    //     return NotFound($"No driver found with id {id}");
    // }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var cacheKey = $"AllDrivers2";

        var cachedDrivers = await _hybridCache.GetOrCreateAsync(cacheKey, async t =>
            {
                var allDrivers = await _unitOfWork.Drivers.GetAllAsync();

                return allDrivers;
            },
            tags:["driver"],
            cancellationToken: cancellationToken
        );

        if (cachedDrivers?.Any() != true)
            return NotFound($"No driver found");

        return Ok(cachedDrivers);
    }

    [HttpPost]
    public async Task<IActionResult> InsertOne(Driver driver)
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