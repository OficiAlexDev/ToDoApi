using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Context
{
    /// <summary>
    /// Creating Database context and extends to DbContext EF core and define Entities with DB SET
    /// </summary>
    /// <param name="options">DB Context Options Builder</param>
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {
        public DbSet<Models.ToDo> ToDo { get; set; }
        public DbSet<User> User { get; set; }
    }
}
