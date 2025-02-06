using JwtApiAuthDotNet9.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtApiAuthDotNet9
{
    public class ApplicationDatabaseContext: DbContext
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options): base(options)       
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}
