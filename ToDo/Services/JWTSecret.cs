using System.Text;

namespace ToDo.Services
{
    /// <summary>
    /// Jason Web Tokens secret services
    /// </summary>
    public static class JWTSecret
    {
        /// <summary>
        /// Return JWT Secret
        /// </summary>
        /// <param name="configuration">IConfiguration used to get JWT Secret</param>
        /// <returns></returns>
        /// <exception cref="Exception">Maybe exception if is missing JWT Secret in appSettings</exception>
        public static byte[] Key(IConfiguration configuration)
        {
            try
            {
                return ASCIIEncoding.ASCII.GetBytes(configuration.GetConnectionString("JWTSecret")!);
            }
            catch (Exception)
            {
                throw new Exception("JWT Secret error on convert to bytes");
            }
        }
    }
}
