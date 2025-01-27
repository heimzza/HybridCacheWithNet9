using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public interface ICommonActions<in T> where T : class
{
    public Task<IActionResult> GetOne(Guid id);
    public Task<IActionResult> GetAll();
    public Task<IActionResult> InsertOne(T driver);
}