using DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerMapping());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    public DbSet<Customer> Customers { get; set; }

}