using System.Text.Json.Serialization;

namespace ToDo.Models.Parameters
{
    public class UserLogin
    {
        required public string Identifier { get; set; }
        required public string Password { get; set; }
    }
}
