using System.Security.Cryptography;
using System.Text;

namespace ToDo.Services
{
    /// <summary>
    /// Password services
    /// </summary>
    public static class Password
    {
        /// <summary>
        /// Generate hasing of password
        /// </summary>
        /// <param name="password">UFT-8 password</param>
        /// <returns>hash of password</returns>
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
