using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Context
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {
        public DbSet<Models.ToDo> ToDo { get; set; }
        public DbSet<User> User { get; set; }
    }
}
