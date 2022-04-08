using Microsoft.EntityFrameworkCore;

namespace EFCore.DependencyInjection.Data;

public class CustomerContext : DbContext
{
    public DbSet<Person> Customers => Set<Person>();
    public DbSet<Order> Orders => Set<Order>();

    public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options)
    { }
}
