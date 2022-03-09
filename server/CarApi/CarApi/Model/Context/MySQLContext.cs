using Microsoft.EntityFrameworkCore;

namespace CarApi.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {
        
        }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) {}
        
        public DbSet<Car> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
