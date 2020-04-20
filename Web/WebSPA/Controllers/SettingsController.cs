using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebSPA.Controllers
{
    public class Setting
    {
        public string webApi { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private IConfiguration _config;
        public SettingsController(
            IConfiguration config)
        {
            _config = config;
        }
        // GET: api/Settings
        [HttpGet]
        public Setting Get()
        {
            return new Setting { webApi = _config["server"] };
        }

       
    }
}
