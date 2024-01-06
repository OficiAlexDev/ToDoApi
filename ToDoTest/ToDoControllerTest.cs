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
using static System.Convert;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace ToDoTest
{
    [TestClass]
    public class ToDoControllerTest()
    {
        static TodoDbContext? dbContext;
        static ToDoController? toDoController;
        static string autorizationToken = String.Empty;
        /// <summary>
        ///   Test create To Do
        /// </summary>
        [TestMethod]
        public async Task CreateToDo()
        {
            IActionResult result = await toDoController!.CreateTodo("Do Test", autorizationToken);
            Assert.AreEqual<int?>(200, ((OkObjectResult)result).StatusCode);
            WriteLine(JsonObject.Parse(((OkObjectResult)result).ToJson())?["Value"]?["message"]);
        }
        /// <summary>
        ///   Test find all to dos from a user
        /// </summary>
        [TestMethod]
        public void Find()
        {
            IActionResult result = toDoController!.Find(autorizationToken);
            Assert.AreEqual<int?>(200, ((OkObjectResult)result).StatusCode);
            WriteLine($"Success find: {JsonObject.Parse(((OkObjectResult)result).ToJson())?["Value"]?["Count"]}");
        }
        /// <summary>
        ///   Test create and find a to do from a user
        /// </summary>
        [TestMethod]
        public async Task CreateAndFindOne()
        {
            string toDoDesc = "Create and find one task id Test";
            IActionResult result = toDoController!.Find((await CreateToDoInMemory(toDoDesc))!.Id, autorizationToken);
            Assert.AreEqual<int?>(200, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual<string>(toDoDesc, JsonObject.Parse(result.ToJson())?["Value"]?["Desc"]?.ToString());
            WriteLine($"To do created: {JsonObject.Parse(result.ToJson())?["Value"]?["Desc"]}");
        }
        /// <summary>
        ///   Test create and update a to do from a user
        /// </summary>
        [TestMethod]
        public async Task CreateAndUpdate()
        {
            string oldToDoDesc = "Old todo!";
            string newToDoDesc = "New todo!";
            ToDo.Models.ToDo? toDo = await CreateToDoInMemory(oldToDoDesc);
            ToDo.Models.ToDo updatedToDo = new()
            {
                Id = toDo!.Id,
                Desc = newToDoDesc,
                Complete = true,
                UserId = toDo.UserId
            };
            IActionResult result = await toDoController!.UpdateToDo(updatedToDo, autorizationToken);
            Assert.AreEqual<int?>(200, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual<int>(toDo.Id, ToInt32(JsonObject.Parse(result.ToJson())?["Value"]?["Id"]?.ToString()));
            Assert.AreEqual<int>(toDo.UserId, ToInt32(JsonObject.Parse(result.ToJson())?["Value"]?["UserId"]?.ToString()));
            Assert.AreNotEqual<string>(oldToDoDesc, JsonObject.Parse(result.ToJson())?["Value"]?["Desc"]?.ToString());
            WriteLine($"To Do updated to: {JsonObject.Parse(result.ToJson())?["Value"]?["Desc"]?.ToString()}");
        }
        /// <summary>
        ///   Test create and dalete a to do from a user
        /// </summary>
        [TestMethod]
        public async Task CreateAndDelete()
        {
            ToDo.Models.ToDo? toDo = await CreateToDoInMemory("ToDo to delete");
            int toDoCount = ToInt32(JsonObject.Parse(toDoController!.Find(autorizationToken).ToJson())?["Value"]?["Count"]?.ToString());
            IActionResult result = await toDoController.DeleteToDo(toDo!.Id, autorizationToken);
            Assert.AreEqual<int?>(200, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual<int?>(toDoCount - 1, ToInt32(JsonObject.Parse(toDoController.Find(autorizationToken).ToJson())?["Value"]?["Count"]?.ToString()));
            Assert.AreNotEqual<int?>(toDoCount, ToInt32(JsonObject.Parse(toDoController.Find(autorizationToken).ToJson())?["Value"]?["Count"]?.ToString()));
            WriteLine($"Num of to do deleted: {((OkObjectResult)result).Value}");
        }
        /// <summary>
        ///   Test fail find a to do from a user by bad token
        /// </summary>
        [TestMethod]
        public void FailFind()
        {
            string badToken = "Bearer: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im5vdHJlYWx1c2VyIiwiSWQiOiI5OTk5IiwibmJmIjoxNzA0NTY0ODk2LCJleHAiOjE3MDQ1NjYzOTYsImlhdCI6MTcwNDU2NDg5Nn0.Wb_OQh1uZ6EbJllVxTR8XUO2AS7YiWJwwRKvMOS4HcY";
            IActionResult result = toDoController!.Find(badToken);
            Assert.AreEqual<int?>(404, ((NotFoundResult)result).StatusCode);
            WriteLine(result.ToJson());
        }
        /// <summary>
        /// Init DB Context and controllers
        /// </summary>
        /// <param name="_testCtx">TestContext</param>
        /// <returns></returns>
        [ClassInitialize]
        public static async Task InitProperties(TestContext _testCtx)
        {
            User testUser = new() { Username = "testToDo", Email = "testToDo@test.com", Password = "1234@#Aa" };
            dbContext = InMemoryDbContext();
            toDoController = new(dbContext);
            AuthController auth = new AuthController(dbContext, new JWTServices(Configuration()));
            await auth.Register(testUser);
            autorizationToken = $"Bearer: {JObject.Parse((await auth.Authentication(new ToDo.Models.Parameters.UserLogin { Identifier = testUser.Username, Password = testUser.Password }) as OkObjectResult).ToJson())["Value"]?["token"]?.ToString()}";
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
        /// <summary>
        /// Create a to do in memory
        /// </summary>
        /// <param name="toDoDesc">Optional desc of to do</param>
        /// <returns></returns>
        private static async Task<ToDo.Models.ToDo?> CreateToDoInMemory(string toDoDesc = "default toDo desk") => JsonConvert.DeserializeObject<ToDo.Models.ToDo>(JsonObject.Parse(((OkObjectResult)(await toDoController!.CreateTodo(toDoDesc, autorizationToken))).ToJson())!["Value"]!["toDo"]!.ToString());
    }
}

