using static System.Convert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using ToDo.Context;
using ToDo.Models;

namespace ToDo.Controllers
{
    [Route("/todo")]
    [Authorize]
    [ApiController]
    public class ToDoController(TodoDbContext db) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Models.ToDo> Find([FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            return user?.ToDos ?? [];
        }

        [HttpPost("id")]
        public IActionResult Find([FromBody] int id, [FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            Models.ToDo? toDo = user?.ToDos?.Where(toDo => toDo.Id == id).FirstOrDefault();
            return toDo != null ? Ok(toDo) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] string desc, [FromHeader] string authorization)
        {
            Models.ToDo toDo = new()
            {
                UserId = IdInBaerer(authorization),
                Desc = desc,
            };
            db.ToDo.Add(toDo);
            await db.SaveChangesAsync();
            return Ok("ToDo created!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateToDo([FromBody] Models.ToDo toDo, [FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            Models.ToDo? oldToDo = user?.ToDos?.Where(toDo => toDo.Id == toDo.Id).FirstOrDefault();
            if (oldToDo != null)
            {
                oldToDo.Desc = toDo.Desc ?? oldToDo.Desc;
                oldToDo.Complete = toDo.Complete ?? oldToDo.Complete;
                db.ToDo.Update(oldToDo);
                await db.SaveChangesAsync();
                return Ok(oldToDo);
            }
            return BadRequest("Something went wrong ToDo not updated!");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo([FromBody] int id, [FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            Models.ToDo? toDo = user?.ToDos?.Where(toDo => toDo.Id == id).FirstOrDefault();
            if (toDo == null)
            {
                return NotFound("ToDo not found!");
            }
            if (toDo.UserId != IdInBaerer(authorization))
            {
                return Unauthorized();
            }
            db.ToDo.Remove(toDo);
            return Ok(await db.SaveChangesAsync());
        }

        private static int IdInBaerer(string authorization)
        {
            return ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(authorization.Split(" ")[1]).Claims.First(claim => claim.Type == "Id").Value);
        }
    }
}

