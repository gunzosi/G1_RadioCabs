using AuthenticationServices.Database;
using AuthenticationServices.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisClient;
using System;
using System.Threading.Tasks;

namespace AuthenticationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly REDISCLIENT _redisClient;
        
        public PasswordResetController(ApplicationDbContext dbContext, IConfiguration configuration, REDISCLIENT client)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _redisClient = client;
        }
        
        // Forgot Password with OTP 
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found"
                });
            }
            
            // OTP 6 digit 
            var otp = new Random().Next(100000, 999999).ToString();
            _redisClient.Set($"otp_{forgotPasswordDto.Email}", otp, TimeSpan.FromMinutes(5));
            
            // Check time if over 5 minutes , alert user to resend OTP
            _redisClient.Publish("otp_event", $"{forgotPasswordDto.Email}|{otp}");
            return Ok(new
            {
                StatusCode = 200,
                Message = "OTP sent to your email"
            });
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var otp = _redisClient.Get($"otp_{resetPasswordDto.Email}");
            if (otp == null || otp != resetPasswordDto.OTP)
            {
                return BadRequest(new { Status = 400, Message = "Invalid OTP" });
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new { Status = 404, Message = "User not found" });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            _redisClient.Remove($"otp_{resetPasswordDto.Email}");

            return Ok(new { Status = 200, Message = "Password reset successfully" });
        }
    }
}
