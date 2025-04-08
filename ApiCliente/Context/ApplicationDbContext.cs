using ApiCliente.Entity;
using Microsoft.EntityFrameworkCore;

namespace ApiCliente.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Cliente { get; set; }
    }
}
