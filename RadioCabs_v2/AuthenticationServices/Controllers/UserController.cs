using AuthenticationServices.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisClient;

namespace AuthenticationServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        
        public UserController(ApplicationDbContext dbContext, IConfiguration configuration, REDISCLIENT _client)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = _client;
        }
        
        
    }
}
