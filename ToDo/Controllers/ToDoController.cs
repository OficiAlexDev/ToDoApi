using static System.Convert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using ToDo.Context;
using ToDo.Models;
using ToDo.Services;
using System.Net;

namespace ToDo.Controllers
{

    [Route("/todo")]
    [Authorize]
    [ApiController]
    public class ToDoController(TodoDbContext db, Redis cache) : ControllerBase
    {

        /// <summary>
        /// Extract user id from JWT
        /// </summary>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        private static int IdInBaerer(string authorization)
        {
            return ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(authorization.Split(" ")[1]).Claims.First(claim => claim.Type == "Id").Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        private async Task<User?> UpsertUserCache(string authorization)
        {
            User? user = db.User.Where(user => user.Id == IdInBaerer(authorization)).Include(user => user.ToDos).FirstOrDefault();
            if (user != null)
            {
                await cache.SetCache(cacheKey(authorization), user);
                return user;
            }
            return null;
        }
        /// <summary>
        /// Define user cache key
        /// </summary>
        /// <param name="authorization">To set key by id</param>
        /// <returns></returns>
        string cacheKey(string authorization) => $"user-{IdInBaerer(authorization)}";
        /// <summary>
        /// Find all to dos from a user
        /// </summary>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Find([FromHeader] string authorization)
        {

            User? user = await cache.GetCache<User>(cacheKey(authorization));
            if (user != null)
            {
                return Ok(new { user?.ToDos, user?.ToDos?.Count });
            }
            user = await UpsertUserCache(authorization);
            if (user != null)
            {
                return Ok(new { user?.ToDos, user?.ToDos?.Count });
            }
            return NotFound();
        }
        /// <summary>
        /// Find a to do from a user
        /// </summary>
        /// <param name="id">To Do Id</param>
        /// <param name="authorization">Bearer Token</param>
        /// <returns></returns>
        [HttpPost("id")]
        public async Task<IActionResult> Find([FromBody] int id, [FromHeader] string authorization)
        {
            User? user = await cache.GetCache<User>(cacheKey(authorization));
            if (user == null)
            {
                user = await UpsertUserCache(authorization);
            }
            user = await cache.GetCache<User>(cacheKey(authorization));
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
            int userId = IdInBaerer(authorization);
            Models.ToDo? sameDesc = db.ToDo.Where(toDo => Equals(toDo.Desc, desc) && Equals(toDo.UserId, userId)).FirstOrDefault();
            if (sameDesc != null)
            {
                return BadRequest($"You already have this to do!");
            }
            Models.ToDo toDo = new()
            {
                UserId = userId,
                Desc = desc,
            };
            db.ToDo.Add(toDo);
            await db.SaveChangesAsync();
            await UpsertUserCache(authorization);
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
            Models.ToDo? oldToDo = user?.ToDos?.Where(oldToDo => Equals(oldToDo.Id, toDo.Id)).FirstOrDefault();
            try
            {
                if (oldToDo != null)
                {
                    oldToDo.Desc = toDo.Desc ?? oldToDo.Desc;
                    oldToDo.Complete = toDo.Complete ?? oldToDo.Complete;
                    db.ToDo.Update(oldToDo);
                    await db.SaveChangesAsync();
                    await UpsertUserCache(authorization);
                    return Ok("To do updated!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong ToDo not updated!");
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
            await db.SaveChangesAsync();
            await UpsertUserCache(authorization);
            return Ok("To do deleted!");
        }
    }
}

