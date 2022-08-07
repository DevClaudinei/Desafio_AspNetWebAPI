using DomainModels.Entities;
using EntityFrameworkCore.AutoHistory.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override int SaveChanges()
    {
        var addedEntities = this.DetectChanges(EntityState.Added);

        this.EnsureAutoHistory();
        var affectedRows = base.SaveChanges();

        this.EnsureAutoHistory(addedEntities);
        affectedRows += base.SaveChanges();

        return affectedRows;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Infrastructure.Data"));
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerBankInfo> CustomerBankInfos { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
}