using Microsoft.EntityFrameworkCore;
using SecurePasswordMVC.Models;

namespace SecurePasswordMVC.Data
{
    public class DB_DataContext : DbContext
    {
        public DB_DataContext(DbContextOptions<DB_DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
