using System.Text;

namespace ToDo.Services
{
    public static class JWTSecret
    {
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
