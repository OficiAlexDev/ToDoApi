using System.Security.Cryptography;
using System.Text;

namespace ToDo.Services
{
    public static class Password
    {
        public static string Hash(string password)
        {
            string hash = "";
            SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).ToList().ForEach(b =>
            {
                hash += b.ToString("x2");
            });
            return hash;
        }
    }
}
