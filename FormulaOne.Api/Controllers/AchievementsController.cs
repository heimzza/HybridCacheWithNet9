using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AchievementsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}