using FormulaOne.Api.Services;
using FormulaOne.DataService.Persistence;
using FormulaOne.Entities.DbSet;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AchievementsController : Controller, ICommonActions<Achievement>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;

    public AchievementsController(IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var cacheKey = $"Achievement_{id}";
        
        var cachedAchievement = _cachingService.Get<Achievement>(cacheKey);

        if (cachedAchievement is not null)
        {
            return Ok(cachedAchievement);
        }
        
        var achievement = await _unitOfWork.Drivers.GetByIdAsync(id);
        
        if (achievement is not null)
        {
            _cachingService.Set(cacheKey, achievement, TimeSpan.FromMinutes(5));
            
            return Ok(achievement);
        }

        return NotFound($"No achievement found with id {id}");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        const string cacheKey = $"Achievements";
        
        var achievements = _cachingService.Get<IEnumerable<Achievement>>(cacheKey);

        if (achievements is not null && achievements.Any())
        {
            return Ok(achievements);
        }
        
        achievements = await _unitOfWork.Achievements.GetAllAsync();

        if (achievements is not null && achievements.Any())
        {
            _cachingService.Set(cacheKey, achievements, TimeSpan.FromMinutes(5));
            
            return Ok(achievements);
        }

        return NotFound($"No achievement found");
    }

    [HttpPost]
    public async Task<IActionResult> InsertOne(Achievement achievement)
    {
        if (achievement is null)
        {
            return BadRequest();
        }

        await _unitOfWork.Achievements.AddAsync(achievement);

        await _unitOfWork.SaveChangesAsync();

        return Ok(achievement);
    }
}