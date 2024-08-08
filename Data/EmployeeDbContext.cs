using Employee.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<EmployeeDetails> Employees { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}
