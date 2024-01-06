namespace ToDo.Models.Parameters
{
    /// <summary>
    /// To Do Model for login
    /// </summary>
    public class UserLogin
    {
        required public string Identifier { get; set; }
        required public string Password { get; set; }
    }
}
