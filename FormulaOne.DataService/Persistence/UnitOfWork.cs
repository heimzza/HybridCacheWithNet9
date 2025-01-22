using FormulaOne.DataService.Repositories;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.DataService.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public IRepository<Driver> Drivers { get; }
    public IRepository<Achievement> Achievements { get; }
    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}