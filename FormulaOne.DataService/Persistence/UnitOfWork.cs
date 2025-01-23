using FormulaOne.DataService.Repositories;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.DataService.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IRepository<Driver> _drivers;
    private IRepository<Achievement> _achievements;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    public IRepository<Driver> Drivers => _drivers ??= new Repository<Driver>(_context);
    public IRepository<Achievement> Achievements => _achievements ??= new Repository<Achievement>(_context);
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
