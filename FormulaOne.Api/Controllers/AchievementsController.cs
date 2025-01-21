using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public class AchievementsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}