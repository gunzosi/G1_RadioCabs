using CompanyServices.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;

namespace CompanyServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminReferenceAction : ControllerBase
    {
        private readonly CompanyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        
        
        public AdminReferenceAction(CompanyDbContext dbContext, IConfiguration configuration, REDISCLIENT redisclient)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = redisclient;
        }
        
        // GET ALL COMPANIES
        [HttpGet("allCompaniesInfo")]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _dbContext.Companies
                    .Include(c => c.CompanyLocationServices)
                    .Include(c => c.CompanyServices)
                    .ToListAsync();

                if (companies == null || companies.Count == 0)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No Companies Found"
                    });
                }

                return Ok(new
                {
                    Status = 200,
                    Message = "No companies found"
                });
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "There is an error for getAllCompanies infomation" ,
                    Detail = ex.Message
                });
            }
        }
        
        // DELETE COMPANIES and All Information Include
        [HttpDelete("company/{id}/delete")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var company = await _dbContext.Companies.FindAsync(id);
                if (company == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Company not found"
                    });
                }
                
                _dbContext.Companies.Remove(company);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Company deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the company",
                    Error = ex.Message
                });
            }
        }
        
        // TEST AUTHORIZE JWT ROLE = ADMIN
        [HttpGet("testAuthorize")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAuthorize()
        {
            return Ok(new
            {
                StatusCode = 200,
                Message = "You are authorized"
            });
        }
        
    }
}
