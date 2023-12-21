using Microsoft.EntityFrameworkCore;

namespace ToDo.Context
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {
        public DbSet<Models.ToDo> ToDo { get; set; }
    }
}
