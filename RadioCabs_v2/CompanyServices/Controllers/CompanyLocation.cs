using CompanyServices.DTOs;
using CompanyServices.Database;
using CompanyServices.Models;
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
        private readonly REDISCLIENT _redisClient;

        public CompanyLocation(CompanyDbContext dbContext, IConfiguration configuration, REDISCLIENT redisClient)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisClient = redisClient;
        }

        // Add Location to Company
        [HttpPost("company/location")]
        public async Task<IActionResult> AddLocation(LocationServiceDto locationServiceDto)
        {
            try
            {
                var existingLocation = await _dbContext.Companies
                    .Include(c => c.CompanyLocationServices)
                    .FirstOrDefaultAsync(cls => cls.CompanyLocationServices
                        .Any(l => l.CityService == locationServiceDto.CityService));

                if (existingLocation != null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "This Location already exists",
                    });
                }

                var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == locationServiceDto.CompanyId);
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
                    CityService = locationServiceDto.CityService
                };

                await _dbContext.CompanyLocationServices.AddAsync(location);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLocationById), new { locationId = location.Id }, location);
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
        
        // Add Array Many at 1 times Location ex : "companyId: 1", cityService : ["city1", "city2", "city3"]
      

        

        // Get Location by ID
        [HttpGet("company/location/{locationId}")]
        public async Task<IActionResult> GetLocationById(int locationId)
        {
            try
            {
                var location = await _dbContext.CompanyLocationServices.FirstOrDefaultAsync(cls => cls.Id == locationId);
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

        // Get All Locations by Company ID
        [HttpGet("company/{companyId}/locations")]
        public async Task<IActionResult> GetLocationsByCompanyId(int companyId)
        {
            try
            {
                var locations = await _dbContext.CompanyLocationServices
                    .Where(cls => cls.CompanyId == companyId)
                    .ToListAsync();

                if (locations.Count == 0)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No locations found",
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

        // Update Location by ID
        [HttpPut("company/location/{locationId}")]
        public async Task<IActionResult> UpdateLocation(int locationId, [FromBody] LocationServiceDto locationServiceDto)
        {
            try
            {
                var location = await _dbContext.CompanyLocationServices.FindAsync(locationId);
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
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while updating the location",
                    Error = ex.Message
                });
            }
        }

        // Delete Location by ID
        [HttpDelete("company/location/{locationId}")]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            try
            {
                var location = await _dbContext.CompanyLocationServices.FindAsync(locationId);
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
