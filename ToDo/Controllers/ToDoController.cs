using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Context;

namespace ToDo.Controllers
{
    [Route("/todo")]
    [ApiController]
    public class ToDoController(TodoDbContext db) : ControllerBase
    {
        [HttpGet()]
        public async Task<IEnumerable<Models.ToDo>> Find()
        {
            return await db.ToDo.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Find(int id)
        {
            Models.ToDo? todo = await db.ToDo.FindAsync(id);
            return todo == null ? NotFound("ToDo not found!") : Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(string desc)
        {
            Models.ToDo toDo = new()
            {
                Desc = desc
            };
            db.Add(toDo);
            await db.SaveChangesAsync();
            return Ok("ToDo created!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateToDo(int id, string? desc, bool? complete)
        {
            Models.ToDo? toDo = await db.ToDo.FindAsync(id);
            if (toDo != null)
            {
                toDo.Desc = desc ?? toDo.Desc;
                toDo.Complete = complete ?? toDo.Complete;
                db.ToDo.Update(toDo);
                await db.SaveChangesAsync();
                return Ok(toDo);
            }
            return BadRequest("ToDo not found!");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            Models.ToDo? toDo = await db.ToDo.FindAsync(id);
            if (toDo == null)
            {
                return BadRequest("ToDo not found!");
            }
            db.ToDo.Remove(toDo);
            return Ok(await db.SaveChangesAsync());
        }
    }
}
