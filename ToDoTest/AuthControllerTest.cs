using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using ToDo.Context;
using ToDo.Models;
using ToDo.Controllers;
using ToDo.Services;
using static System.Console;

namespace ToDoTest
{
    [TestClass]
    public class AuthControllerTest()
    {
        static TodoDbContext? dbContext;
        static AuthController? authController;
        static readonly User testUser = new() { Username = "test1", Email = "test1@test.com", Password = "1234@#Aa" };

        /// <summary>
        /// Test auth controller register user (User must be created)
        /// </summary>
        [TestMethod]
        public async Task SuccessfulRegister()
        {
            IActionResult result = await authController!.Register(new() { Username = "test2register", Email = "test2register@test.com", Password = "1234@#Aa" });
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            WriteLine(((OkObjectResult)result).Value);
        }
        /// <summary>
        /// Test fail register (User must be not created by same email)
        /// </summary>
        [TestMethod]
        public async Task FailRegisterByEmail()
        {
            IActionResult result = await authController!.Register(new() { Username = "test2", Email = "test1@test.com", Password = "1234@#Aa" });
            WriteLine(JObject.Parse(result.ToJson())["Value"]);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
        }
        /// <summary>
        /// Test fail register (User must be not created by same name)
        /// </summary> 
        [TestMethod]
        public async Task FailRegisterByName()
        {
            IActionResult result = await authController!.Register(new() { Username = "test1", Email = "test2@test.com", Password = "1234@#Aa" });
            WriteLine(JObject.Parse(result.ToJson())["Value"]);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
        }
        /// <summary>
        /// Test fail register (User must be not created by weakness password)
        /// </summary> 
        [TestMethod]
        public async Task FailRegisterByPass()
        {
            IActionResult result = await authController!.Register(new() { Username = "test2", Email = "test2@test.com", Password = "12345678" });
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            result = await authController.Register(new() { Username = "test2", Email = "test2@test.com", Password = "Ab3DEFGH" });
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            result = await authController.Register(new() { Username = "test2", Email = "test2@test.com", Password = "Aa1#$" });
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            result = await authController.Register(new() { Username = "test2", Email = "test2@test.com", Password = "Aa123456" });
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            result = await authController.Register(new() { Username = "test2", Email = "test2@test.com", Password = "!@#$%*&#" });
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            WriteLine(JObject.Parse(result.ToJson())["Value"]);
        }

        /// <summary>
        /// Test success login (With username )
        /// </summary>
        [TestMethod]
        public async Task LoginByUsername()
        {
            ActionResult result = await authController!.Authentication(new() { Identifier = testUser.Username, Password = testUser.Password });
            WriteLine(JObject.Parse(result.ToJson())["Value"]?["signedWith"]);
            Assert.AreEqual<string?>("Username", JObject.Parse(result.ToJson())["Value"]?["signedWith"]?.ToString());
            Assert.AreEqual<string?>(testUser.Username, JObject.Parse(result.ToJson())["Value"]?["user"]?["username"]?.ToString());
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        /// <summary>
        /// Test success login (With email)
        /// </summary>
        [TestMethod]
        public async Task LoginByEmail()
        {
            ActionResult result = await authController!.Authentication(new() { Identifier = testUser.Email, Password = testUser.Password });
            WriteLine(JObject.Parse(result.ToJson())["Value"]?["signedWith"]);
            Assert.AreEqual<string?>("Email", JObject.Parse(result.ToJson())["Value"]?["signedWith"]?.ToString());
            Assert.AreEqual<string?>(testUser.Username, JObject.Parse(result.ToJson())["Value"]?["user"]?["username"]?.ToString());
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }
        /// <summary>
        /// Init DB Context and controllers
        /// </summary>
        /// <param name="_testCtx">TestContext</param>
        /// <returns></returns>
        [ClassInitialize]
        public static async Task InitProperties(TestContext _testCtx)
        {
            dbContext = InMemoryDbContext();
            authController = new(dbContext, new JWTServices(Configuration()));
            await authController.Register(testUser);
        }
        /// <summary>
        /// Create a Db context options to run test using in memory DB
        /// </summary>
        /// <returns></returns>
        private static TodoDbContext InMemoryDbContext() =>
            new(new DbContextOptionsBuilder<TodoDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDB")
                 .Options
                );
        /// <summary>
        /// Create IConfiguration Instance
        /// </summary>
        /// <returns>IConfiguration</returns>
        private static IConfiguration Configuration() =>
            new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: new Dictionary<string, string?> {
                        {"ConnectionStrings:JWTSecret","7C81e1N433W338yayOIH8r8OQZvfYjJ3DN7488hpy00u40If" }
                    }
                )
                 .Build();
    }
}
