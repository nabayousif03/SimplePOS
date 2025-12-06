using Microsoft.EntityFrameworkCore;
using SimplePOS.Models;

namespace SimplePOS;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<orders> Orders { get; set; }
}