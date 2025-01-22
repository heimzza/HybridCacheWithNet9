using FormulaOne.DataService.Repositories;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.DataService.Persistence;

public interface IUnitOfWork
{
    IRepository<Driver> Drivers { get; }
    IRepository<Achievement> Achievements { get; }
    Task<int> SaveChangesAsync();
}