using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.Entities.DbSet;

public class AppDbContext: DbContext
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    private static string DbPath => "app.db";

    public AppDbContext() { }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}

public class Driver(string name, string team, int birthYear)
{
    public Guid Id { get; set; }
    [MaxLength(20)]
    public string Name { get; set; } = name;
    [MaxLength(20)]
    public string Team { get; set; } = team;
    public int BirthYear { get; set; } = birthYear;
}

public class Achievement(string driverId, string place, Achievement.MedalType type, int year)
{
    public Guid Id { get; set; }
    public string DriverId { get; set; } = driverId;
    public string Place { get; set; } = place;
    public MedalType Type { get; set; } = type;
    public int Year { get; set; } = year;

    public enum MedalType
    {
        Gold = 1,
        Silver = 2,
        Bronze = 3
    };
}