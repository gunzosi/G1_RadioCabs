using CompanyServices.Database;
using CompanyServices.DTOs;
using CompanyServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;

namespace CompanyServices.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly CompanyDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly REDISCLIENT _redisclient;

    public PaymentController(CompanyDbContext dbContext, IConfiguration configuration, REDISCLIENT redisclient)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _redisclient = redisclient;
    }
    
    // CREATE
    [HttpPost("company/payment")]
    public async Task<IActionResult> AddPayment(PaymentDto paymentDto)
    {
        try
        {

            var payment = new Payment
            {
                CompanyId = paymentDto.Id,
                Amount = paymentDto.Amount,
                ContentPayment = paymentDto.ContentPayment,
                PaymentAt = DateTime.UtcNow,
                IsPayment = true,
            };

            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "Payment registered successfully"
            });

        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                Status = 404,
                Message = "Company REGISTER request ERROR at CompanyAuthController - /api/v1/company/register",
                Error = e.Message,
                InnerError = e.InnerException?.Message
            });
        }

    }
    
    
    // UPDATE update amount, contentPayment với paymentAt
    [HttpPut("company/payment/{paymentId}")]
    public async Task<IActionResult> UpdatePayment(int paymentId, PaymentDto paymentDto)
    {
        try
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null)
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "Payment not found"
                });
            }

            payment.Amount = paymentDto.Amount;
            payment.ContentPayment = paymentDto.ContentPayment;
            payment.PaymentAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "Payment updated successfully"
            });

        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                Status = 404,
                Message = "Company REGISTER request ERROR at CompanyAuthController - /api/v1/company/register",
                Error = e.Message,
                InnerError = e.InnerException?.Message
            });
        }
    }

}