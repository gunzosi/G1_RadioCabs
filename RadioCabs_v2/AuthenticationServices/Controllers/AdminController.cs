using System.Data.Common;
using AuthenticationServices.Database;
using AuthenticationServices.DTOs;
using AuthenticationServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;
using StackExchange.Redis;

namespace AuthenticationServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        // private readonly IBlobServices _blobServices;
        
        public AdminController(ApplicationDbContext dbContext, IConfiguration configuration, REDISCLIENT client)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = client;
        }
        
        // CRUD Driver
        [HttpGet("getAllDrivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var drivers = await _dbContext.Drivers.Include(d => d.DriverInfo).ToListAsync();
                return Ok(new
                {
                    Status = 200,
                    Data = drivers
                });
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = 500,
                    Message = "Error! Can't get data from api/v1/admin/getAllDrivers - AdminController/AuthenticationServices"
                });
            }
        }

        [HttpGet("getDriverById/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            try
            {
                var driver = await _dbContext.Drivers.FindAsync(id);
                if (driver == null)
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Driver not found"
                    });
                }
                return Ok(new
                {
                    Status = 200,
                    Data = driver
                });
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = 500,
                    Message = "Error! Can't get data from api/v1/admin/getDriverById - AdminController/AuthenticationServices"
                });
            }
        }
        
        [HttpPut("updateDriver/{id}")]
        public async Task<IActionResult> UpdateDriver(int id,[FromForm] DriverInfoDto driverDto)
        {
            
                var driver = await _dbContext
                                        .Drivers
                                        .Include(d => d.DriverInfo)
                                        .FirstOrDefaultAsync(d => d.Id == id);
                if (driver == null)
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Driver not found"
                    });
                }
                if (!string.IsNullOrEmpty(driverDto.DriverFullName))
                {
                    driver.DriverFullName = driverDto.DriverFullName;
                }
                if (!string.IsNullOrEmpty(driverDto.DriverEmail))
                {
                    driver.DriverEmail = driverDto.DriverEmail;
                }
                if (!string.IsNullOrEmpty(driverDto.DriverLicense))
                {
                    driver.DriverInfo.DriverLicense = driverDto.DriverLicense;
                }
                if (!string.IsNullOrEmpty(driverDto.DriverLicense))
                {
                    driver.DriverInfo.DriverLicense = driverDto.DriverLicense;
                }
                if (!string.IsNullOrEmpty(driverDto.Address))
                {
                    driver.DriverInfo.Address = driverDto.Address;
                }
                if (!string.IsNullOrEmpty(driverDto.Street))
                {
                    driver.DriverInfo.Address = driverDto.Street;
                }
                if (!string.IsNullOrEmpty(driverDto.Ward))
                {
                    driver.DriverInfo.Ward = driverDto.Ward;
                }
                if (!string.IsNullOrEmpty(driverDto.City))
                {
                    driver.DriverInfo.City = driverDto.City;
                }
                
                // if (driverDto.DriverPersonalImage != null)
                // {
                //     var contentType = BlobContentTypes.GetContentType(driverDto.DriverPersonalImage);
                //     driver.DriverInfo.DriverPersonalImage = await _blobServices.UploadBlobWithContentTypeAsync(driverDto.DriverPersonalImage, contentType);
                // }

                // if (driverDto.DriverLicenseImage != null)
                // {
                //     var contentType = BlobContentTypes.GetContentType(driverDto.DriverLicenseImage);
                //     driver.DriverInfo.DriverLicenseImage = await _blobServices.UploadBlobWithContentTypeAsync(driverDto.DriverLicenseImage, contentType);
                // }
                
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    Status = 200,
                    Message = "Driver updated successfully"
                });
                    
            } catch (DbUpdateConcurrencyException e)
            {
                if (!_dbContext.Drivers.Any(e => e.Id == id))
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Driver not found - api/v1/admin/updateDriver - AdminController/AuthenticationService"
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = 500,
                    Message = "Error! Can't update data from api/v1/admin/updateDriver - AdminController/AuthenticationServices"
                });
            }
        }
        
        [HttpDelete("deleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            // Delete Driver and DriverInfo
            var driver = await _dbContext.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "Driver not found"
                });
            }
            try
            {
                _dbContext.Drivers.Remove(driver);
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    Status = 200,
                    Message = "Driver deleted successfully"
                });
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = 500,
                    Message = "Error! Can't delete data from api/v1/admin/deleteDriver - AdminController/AuthenticationServices"
                });
            }
        }
        
        // CRUD User 
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                if (users != null)
                {
                    return Ok(new
                    {
                        Status = 200,
                        Data = users
                    });

                }
                return NotFound(new
                {
                    Status = 404,
                    Message = "There is no Users in Database"
                });
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = 500,
                    Message = "Error! Can't get data from api/v1/admin/getAllUsers - AdminController/AuthenticationServices"
                });
            }
        }
        
        // Test Authorize Method
        [HttpGet("testAuthorize")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminAuthorize()
        {
            try
            {
                return Ok(new
                {
                    Message = "Admin is authorized to access this method"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    Message = "Error by Server , check try block at Authorize method of AuthenticationService.DriverController",
                    Details = ex.Message
                });
            }
        }
    }
}
