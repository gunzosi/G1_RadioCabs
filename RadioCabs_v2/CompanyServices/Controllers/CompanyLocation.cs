using AuthenticationServices.DTOs;
using CompanyServices.Database;
using CompanyServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;

namespace CompanyServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyLocation : ControllerBase
    {
        private readonly CompanyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        
        
        public CompanyLocation(CompanyDbContext dbContext, IConfiguration configuration, REDISCLIENT redisclient)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = redisclient;
        }
        
        // CRUD operations for company location
        [HttpPost("company/location")]
        public async Task<IActionResult> AddLocation(LocationServiceDto locationService)
        {
            try
            {
                var existingLocation = await _dbContext
                    .Companies
                    .Include(c => c.CompanyLocationServices)
                    .FirstOrDefaultAsync(cls => cls.CompanyLocationServices
                        .Any(l => l.CityService == locationService.CityService));

                if (existingLocation != null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "This Location already exists",
                    });
                }

                var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == locationService.CompanyId);
                if (company == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Company not found",
                    });
                }

                var location = new CompanyLocationService
                {
                    CompanyId = company.Id,
                    CityService = locationService.CityService
                };

                await _dbContext.CompanyLocationServices.AddAsync(location);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLocationById), new { id = location.Id }, location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while adding the location",
                    Error = ex.Message
                });
            }
        }
        
        [HttpGet("company/location/{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            try
            {
                var location = await _dbContext.CompanyLocationServices.FirstOrDefaultAsync(cls => cls.Id == id);
                if (location == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Location not found",
                    });
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching the location",
                    Error = ex.Message
                });
            }
        }
        
        // Get All Location At Company id 
        [HttpGet("company/{id}/locations")]
        public async Task<IActionResult> GetLocationsByCompanyId(int id)
        {
            try
            {
                var locations = await _dbContext.CompanyLocationServices.Where(cls => cls.CompanyId == id).ToListAsync();
                if (locations.Count == 0)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No location found",
                    });
                }

                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching the locations",
                    Error = ex.Message
                });
            }
        }

        [HttpPut("company/location/{id}")]
        public async Task<IActionResult> UpdateLocation(int id, LocationServiceDto locationServiceDto)
        {
            try
            {
                var location = await _dbContext.CompanyLocationServices.FindAsync(id);
                if (location == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Location not found",
                    });
                }
                
                location.CityService = locationServiceDto.CityService;
                _dbContext.CompanyLocationServices.Update(location);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Location updated successfully",
                    Data = location
                });
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while updating the location",
                    Error = ex.Message
                });
            }
        }
        
        [HttpDelete("company/location/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var location = await _dbContext.CompanyLocationServices.FindAsync(id);
                if (location == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Location not found",
                    });
                }

                _dbContext.CompanyLocationServices.Remove(location);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Location deleted successfully",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the location",
                    Error = ex.Message
                });
            }
        }
        
        
    }
}
