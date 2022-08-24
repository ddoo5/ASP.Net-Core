using ContractControlCentre.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContractControlCentre.DB.Connections
{
    public class DbConnection : DbContext
    {
        public DbSet<PersonModelEntity> Persons { get; set; }
        public DbSet<EmployeeModelEntity> Employees { get; set; }


        public DbConnection()
        {
            Database.EnsureDeleted();      //drop
            Database.EnsureCreated();     //up
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)      //connection
        {
            optionsBuilder
            .UseNpgsql("Host=localhost;Port=5432;Database=testdb;Username=pguser;Password=pgpwd;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)      //create tables
        {
            modelBuilder.Entity<PersonModelEntity>();
            modelBuilder.Entity<EmployeeModelEntity>();
        }
    }
}

