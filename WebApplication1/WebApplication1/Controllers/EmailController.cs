using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private static DateTime _lastRequestTime;
        private static readonly object _lock = new object();

        [HttpPost]
        public IActionResult Post([FromBody] EmailModel email)
        {
            lock (_lock)
            {
                var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
                if (timeSinceLastRequest < TimeSpan.FromSeconds(3))
                {
                    return StatusCode(429, "Too many requests");
                }
                _lastRequestTime = DateTime.UtcNow;
            }

            var response = new
            {
                email = email.Email,
                date = DateTime.UtcNow
            };

            return Ok(response);
        }
    }

    public class EmailModel
    {
        public string Email { get; set; }
    }
}

