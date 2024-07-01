using Microsoft.EntityFrameworkCore;

namespace ContaBancariaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContaBancaria> Contas {  get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContaBancaria>().HasKey(conta => conta.Id);
        }
    }
}
