using AuthenticationServices.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;

namespace AuthenticationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverCompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        
        public DriverCompanyController(ApplicationDbContext dbContext, IConfiguration configuration, REDISCLIENT client)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = client;
        }
        
        // Driver Register Comapny by ID - ex: api/drivercompany/1/register
        // FORM AT DRIVER PAGE TO REGISTER COMPANY 
        // EX: Driver login -> Company Page -> Click Register -> Fill Form -> Submit
        // [HttpPost("{companyId}/register")]
        
        [HttpGet("company/{companyId}/drivers")]
        public async Task<IActionResult> GetDriversByCompanyId(string companyId)
        {
            try
            {
                var drivers = await _dbContext.Drivers.Where(d => d.CompanyId == companyId).ToListAsync();
                if (drivers == null || !drivers.Any())
                {
                    return NotFound(new
                    {
                        Status = 404, 
                        Message = "No drivers found for this company." 
                    });
                }

                return Ok(new { Status = 200, Message = "Success", Drivers = drivers });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new { Status = 400, Message = "Error: " + e.Message });
            }
        }

        
        
    }
}
