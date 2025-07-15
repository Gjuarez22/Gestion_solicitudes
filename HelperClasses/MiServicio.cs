
using Microsoft.Extensions.Configuration;

namespace GestionSolicitud.HelperClasses
{
    public class MiServicio
    {
        private readonly IConfiguration _configuration;

        public MiServicio(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionSting(string keysetting)
        {
            return _configuration["ConnectionStrings:" + keysetting];
        }
    }
}
