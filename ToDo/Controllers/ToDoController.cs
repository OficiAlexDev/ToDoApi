using static System.Convert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using ToDo.Context;
using ToDo.Models;
using NuGet.Protocol;

namespace ToDo.Controllers
{
    [Route("/todo")]
    [Authorize]
    [ApiController]
    public class ToDoController(TodoDbContext db) : ControllerBase
    {
        /// <summary>
        /// Find all to dos from a user
        /// </summary>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Find([FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            return user != null ? Ok(new { user?.ToDos, Count = user?.ToDos?.Count }) : NotFound();
        }
        /// <summary>
        /// Find a to do from a user
        /// </summary>
        /// <param name="id">To Do Id</param>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        [HttpPost("id")]
        public IActionResult Find([FromBody] int id, [FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            Models.ToDo? toDo = user?.ToDos?.Where(toDo => toDo.Id == id).FirstOrDefault();
            return toDo != null ? Ok(toDo) : NotFound("Something gets wrong to do not found!");
        }
        /// <summary>
        /// Create a to do
        /// </summary>
        /// <param name="desc">To do description</param>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
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
            return Ok(new
            {
                message = "ToDo created!",
                toDo
            });
        }
        /// <summary>
        /// Update to do
        /// </summary>
        /// <param name="toDo">New to do</param>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateToDo([FromBody] Models.ToDo toDo, [FromHeader] string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            Models.ToDo? oldToDo = user?.ToDos?.Where(oldToDo => Equals(oldToDo.Id,toDo.Id)).FirstOrDefault();
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
        /// <summary>
        /// Delete one to do
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
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
        /// <summary>
        /// Extract user id from JWT
        /// </summary>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        private static int IdInBaerer(string authorization)
        {
            return ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(authorization.Split(" ")[1]).Claims.First(claim => claim.Type == "Id").Value);
        }
    }
}

