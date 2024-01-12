using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using ToDo.Context;
using ToDo.Models;
using ToDo.Models.Parameters;
using ToDo.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDo.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController(TodoDbContext db, JWTServices jwt) : ControllerBase
    {
        //Regex to valid data
        readonly string passwordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$";
        readonly string excludeUsernamePattern = @"[\W]";
        readonly string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="newUser">User credentials</param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {

            User? user = db.User.Where(user => Equals(user.Username, newUser.Username)).FirstOrDefault();
            User? user1 = db.User.Where(user => Equals(user.Email, newUser.Email)).FirstOrDefault();
            if (user != null)
            {
                return BadRequest(new { status = "This username already in use!", code = 1 });
            }
            if (user1 != null)
            {
                return BadRequest(new { status = "This email already in use!", code = 2 });
            }
            if (!Regex.IsMatch(newUser.Password, passwordPattern))
            {
                return BadRequest(new { status = "The password is not strong enough!", code = 3 });
            }
            if (Regex.IsMatch(newUser.Username, excludeUsernamePattern) || newUser.Username.Length < 4)
            {
                return BadRequest(new { status = "The username must contain only letters and numbers and more than 4 characters!", code = 4 });
            }
            if (!Regex.IsMatch(newUser.Email, emailPattern))
            {
                return BadRequest(new { status = "The email is not valid!", code = 5 });
            }
            db.User.Add(new()
            {
                Username = newUser.Username.ToLower(),
                Email = newUser.Email.ToLower(),
                Password = Password.Hash(newUser.Password),
            });
            await db.SaveChangesAsync();
            return Ok(new { status = "User creted!", code = 0 });
        }
        /// <summary>
        /// Login existent user
        /// </summary>
        /// <param name="userAuth">User credentials</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Authentication([FromBody] UserLogin userAuth)
        {
            userAuth.Identifier = userAuth.Identifier.ToLower();
            User? user;
            user = await db.User.Where(user =>
            Regex.IsMatch(userAuth.Identifier, emailPattern) ?
            Equals(user.Email, userAuth.Identifier.ToLower()) :
            Equals(user.Username, userAuth.Identifier.ToLower())
            ).FirstOrDefaultAsync();
            if (user == null || user.Password != Password.Hash(userAuth.Password))
            {
                return BadRequest($"Username or password invalid!");
            }
            return Ok(
                new
                {
                    token = jwt.GenerateToken(user),
                    user = new
                    {
                        id = user.Id,
                        username = user.Username,
                    },
                    signedWith = Regex.IsMatch(userAuth.Identifier, emailPattern) ? "Email" : "Username"
                }
            );
        }
    }
}

