using CompanyServices.Database;
using CompanyServices.DTOs;
using CompanyServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedisClient;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverOfCompanyController : ControllerBase
    {
        private readonly CompanyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisclient;
        private readonly HttpClient _httpClient;
        // Default URL
        private readonly string _defaultUrl = "http://localhost:11902";
        
        
        public DriverOfCompanyController(CompanyDbContext dbContext, IConfiguration configuration, REDISCLIENT redisclient, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisclient = redisclient;
            _httpClient = httpClient;
        }
        
        // Get All Drivers of Company - ex: api/driverofcompany/1/drivers
        // [HttpGet("{companyId}/drivers")]
        // public IActionResult GetDriversOfCompany(int companyId)
        // {
        //     try
        //     {
        //         var company = _dbContext.Companies.Find(companyId);
        //         if (company == null || company.DriversId == null)
        //         {
        //             return NotFound("Company or Drivers not found.");
        //         }
        //
        //         return Ok(new
        //         {
        //             StatusCode = 200,
        //             Message = "Success",
        //             Drivers = company.DriversId.Select(driverId => new
        //             {
        //                 DriverId = driverId
        //             })
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }

        [HttpGet("{companyId}/drivers")]
        public async Task<IActionResult> GetDriversOfCompany(int companyId)
        {
            // /api/DriverCompany/company/1/drivers
            var url = $"{_defaultUrl + "/api/DriverCompany/" + companyId + "/drivers"}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
                var drivers = await response.Content.ReadAsStringAsync();
                return Ok(new
                {
                    Status = 200,
                    Message = "Success",
                    Drivers = drivers
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { Status = 400, Message = "Error: " + e.Message });
            }
        }
    }
}
