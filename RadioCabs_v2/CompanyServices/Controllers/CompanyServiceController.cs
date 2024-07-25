using CompanyServices.DTOs;
using CompanyServices.Database;
using CompanyServices.DTOs;
using CompanyServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;

namespace CompanyServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyServiceController : ControllerBase
    {
        private readonly CompanyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        
        
        public CompanyServiceController(CompanyDbContext dbContext, IConfiguration configuration, REDISCLIENT redisclient)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = redisclient;
        }

        [HttpPost("company/service/create")]
        public async Task<IActionResult> CreateServices([FromBody] CompanyServicesDto servicesDto)
        {
            try
            {
                var company = await _dbContext.Companies.FindAsync(servicesDto.CompanyId);
                if (company == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Company not found"
                    });
                }

                var companyService = new CompanyService
                {
                    CompanyId = servicesDto.CompanyId,
                    ServiceType = servicesDto.ServiceType
                };

                await _dbContext.CompanyServices.AddAsync(companyService);
                await _dbContext.SaveChangesAsync();

                var response = new CompanyServiceResponseDto()
                {
                    Id = companyService.Id,
                    ServiceType = companyService.ServiceType
                };

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Services created successfully",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while creating services",
                    Error = ex.Message
                });
            }
        }

        
        // GET ALL SERVICE BY COMPANY ID
        [HttpGet("company/{companyId}/services")]
        public async Task<IActionResult> GetServicesByCompanyId(int companyId)
        {
            try
            {
                var services = await _dbContext.CompanyServices
                    .Where(s => s.CompanyId == companyId)
                    .Select(s => new CompanyServiceResponseDto
                    {
                        Id = s.Id,
                        ServiceType = s.ServiceType
                    })
                    .ToListAsync();
        
                if (services == null || services.Count == 0)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No services found for this company"
                    });
                }

                return Ok(new
                {
                    Status = 200,
                    CompanyId = companyId,
                    Data = services
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching the services",
                    Error = ex.Message
                });
            }
        }

        
         // Update services of a company
        // [HttpPut("update/{companyId}")]
        // public async Task<IActionResult> UpdateServices(int companyId, [FromBody] List<CompanyServicesDto> serviceDtos)
        // {
        //     try
        //     {
        //         var company = await _dbContext
        //             .Companies
        //             .Include(c => c.CompanyServices)
        //             .FirstOrDefaultAsync(c => c.Id == companyId);
        //         if (company == null)
        //         {
        //             return NotFound(new
        //             {
        //                 StatusCode = 404,
        //                 Message = "Company not found"
        //             });
        //         }
        //
        //         // Remove existing services not in the new list
        //         var existingServices = company.CompanyServices.ToList();
        //         foreach (var existingService in existingServices)
        //         {
        //             if (!serviceDtos.Any(s => s.ServiceType == existingService.ServiceType))
        //             {
        //                 _dbContext.CompanyServices.Remove(existingService);
        //             }
        //         }
        //
        //         // Update or add new services
        //         foreach (var serviceDto in serviceDtos)
        //         {
        //             var existingService = existingServices.FirstOrDefault(s => s.ServiceType == serviceDto.ServiceType);
        //             if (existingService != null)
        //             {
        //                 existingService.ServiceType = serviceDto.ServiceType;
        //             }
        //             else
        //             {
        //                 var newService = new CompanyService
        //                 {
        //                     CompanyId = companyId,
        //                     ServiceType = serviceDto.ServiceType
        //                 };
        //                 await _dbContext.CompanyServices.AddAsync(newService);
        //             }
        //         }
        //
        //         await _dbContext.SaveChangesAsync();
        //
        //         return Ok(new
        //         {
        //             StatusCode = 200,
        //             Message = "Services updated successfully",
        //             Data = company.CompanyServices
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new
        //         {
        //             StatusCode = 500,
        //             Message = "An error occurred while updating the services",
        //             Error = ex.Message
        //         });
        //     }
        // }
        
        // Delete a service by ID
        // [HttpDelete("delete/{serviceId}")]
        // public async Task<IActionResult> DeleteService(int serviceId)
        // {
        //     try
        //     {
        //         var service = await _dbContext.CompanyServices.FindAsync(serviceId);
        //         if (service == null)
        //         {
        //             return NotFound(new
        //             {
        //                 StatusCode = 404,
        //                 Message = "Service not found"
        //             });
        //         }
        //
        //         _dbContext.CompanyServices.Remove(service);
        //         await _dbContext.SaveChangesAsync();
        //
        //         return Ok(new
        //         {
        //             StatusCode = 200,
        //             Message = "Service deleted successfully"
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new
        //         {
        //             StatusCode = 500,
        //             Message = "An error occurred while deleting the service",
        //             Error = ex.Message
        //         });
        //     }
        // }

        // Delete all services of a company
        // [HttpDelete("company/{companyId}/delete-all")]
        // public async Task<IActionResult> DeleteAllServices(int companyId)
        // {
        //     try
        //     {
        //         var services = await _dbContext.CompanyServices.Where(s => s.CompanyId == companyId).ToListAsync();
        //         if (services == null || services.Count == 0)
        //         {
        //             return NotFound(new
        //             {
        //                 StatusCode = 404,
        //                 Message = "No services found for this company"
        //             });
        //         }
        //
        //         _dbContext.CompanyServices.RemoveRange(services);
        //         await _dbContext.SaveChangesAsync();
        //
        //         return Ok(new
        //         {
        //             StatusCode = 200,
        //             Message = "All services deleted successfully"
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new
        //         {
        //             StatusCode = 500,
        //             Message = "An error occurred while deleting the services",
        //             Error = ex.Message
        //         });
        //     }
        // }
        
        // Update a specific service of a company
        [HttpPut("company/{companyId}/service/{serviceId}")]
        public async Task<IActionResult> UpdateService(int companyId, int serviceId, [FromBody] UpdateCompanyServiceDto serviceDto)
        {
            try
            {
                var companyService = await _dbContext.CompanyServices
                    .FirstOrDefaultAsync(s => s.CompanyId == companyId && s.Id == serviceId);

                if (companyService == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Service not found"
                    });
                }

                if (!string.IsNullOrEmpty(serviceDto.ServiceType))
                {
                    companyService.ServiceType = serviceDto.ServiceType;
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Service updated successfully",
                    Data = companyService
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while updating the service",
                    Error = ex.Message
                });
            }
        }
        
        
        [HttpDelete("delete/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            try
            {
                var service = await _dbContext.CompanyServices.FindAsync(serviceId);
                if (service == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Service not found"
                    });
                }

                _dbContext.CompanyServices.Remove(service);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Service deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the service",
                    Error = ex.Message
                });
            }
        }

        
        [HttpDelete("company/{companyId}/delete-all")]
        public async Task<IActionResult> DeleteAllServices(int companyId)
        {
            try
            {
                var services = await _dbContext.CompanyServices.Where(s => s.CompanyId == companyId).ToListAsync();
                if (services == null || services.Count == 0)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No services found for this company"
                    });
                }

                _dbContext.CompanyServices.RemoveRange(services);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "All services deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the services",
                    Error = ex.Message
                });
            }
        }

    }
}
