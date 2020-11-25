using Microsoft.EntityFrameworkCore;
using TestData;

namespace EFCore
{
    public class PeopleContext : DbContext
    {
        public const string FileName = "data.db";

        public PeopleContext()
            : this(new DbContextOptionsBuilder<PeopleContext>().UseSqlite($"Data Source={FileName}").Options)
        { }

        public PeopleContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Person> People => Set<Person>();
    }
}
