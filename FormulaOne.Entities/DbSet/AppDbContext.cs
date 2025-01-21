using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.Entities.DbSet;

public class AppDbContext: DbContext
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Driver> Achievements { get; set; }
    private static string DbPath => "app.db";

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}

public class Driver(string name, string team, string color, int birthYear)
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string Name { get; set; } = name;
    [MaxLength(20)]
    public string Team { get; set; } = team;
    public int BirthBirthYear { get; set; } = birthYear;
}

public class Achievement(string driverId, string place, Achievement.Type type, int year)
{
    public int Id { get; set; }
    public string Place { get; set; } = place;
    public string DriverId { get; set; } = driverId;
    public Type Type { get; set; } = type;
    public int Year { get; set; } = year;

    public enum Type
    {
        Gold = 1,
        Silver = 2,
        Bronze = 3
    };
}