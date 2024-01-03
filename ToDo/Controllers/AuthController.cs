using Microsoft.AspNetCore.Mvc;
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
    public class AuthController(TodoDbContext db, IConfiguration configuration) : ControllerBase
    {
        private readonly JWTServices jwt = new(configuration);
        //Regex to valid data
        readonly string passwordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$";
        readonly string excludeUsernamePattern = @"[\W]";
        readonly string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        [HttpPost("signup")]
        public async Task<IActionResult> SetupADM([FromBody] User newUser)
        {

            User? user = db.User.Where(user => Equals(user.Username, newUser.Username)).FirstOrDefault();
            User? user1 = db.User.Where(user => Equals(user.Email, newUser.Email)).FirstOrDefault();
            if (user != null || user1 != null)
            {
                return BadRequest("This user already exists!");
            }
            if (!Regex.IsMatch(newUser.Password, passwordPattern))
            {
                return BadRequest("The password is not strong enough!");
            }
            if (Regex.IsMatch(newUser.Username, excludeUsernamePattern))
            {
                return BadRequest("The username must contain only letters and numbers!");
            }
            if (!Regex.IsMatch(newUser.Email, emailPattern))
            {
                return BadRequest("The email is not valid!");
            }
            db.User.Add(new()
            {
                Username = newUser.Username.ToLower(),
                Email = newUser.Email.ToLower(),
                Password = Password.Hash(newUser.Password),
            });
            await db.SaveChangesAsync();
            return Ok("User creted");
        }

        [HttpPost]
        public ActionResult AuthenticationAsync([FromBody] UserLogin userAuth)
        {
            userAuth.Identifier = userAuth.Identifier.ToLower();
            User? user;
            user = db.User.Where(user =>
            Regex.IsMatch(userAuth.Identifier, emailPattern) ?
            Equals(user.Email, userAuth.Identifier.ToLower()) :
            Equals(user.Username, userAuth.Identifier.ToLower())
            ).FirstOrDefault();
            if (user == null || user.Password != Password.Hash(userAuth.Password))
            {
                return NotFound($"Username or password invalid!");
            }
            user.Password = "";
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

