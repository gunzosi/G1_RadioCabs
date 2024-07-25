using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FeedbackServices.DTOs;
using FeedbackServices.Models;
using FeedbackServices.Services;
using Microsoft.AspNetCore.Mvc;
using RedisClient;
using System;
using System.Threading.Tasks;

namespace FeedbackServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackRepository _repository;
        private readonly REDISCLIENT _redisclient;

        public FeedbackController(FeedbackRepository repository, REDISCLIENT redisclient)
        {
            _repository = repository;
            _redisclient = redisclient;
        }

        // READ ALL FEEDBACK từ ADMIN
        [HttpGet("feedback/GetAllFeedback")]
        public async Task<IActionResult> GetAllFeedback()
        {
            try
            {
                var feedbacks = await _repository.GetAllAsync();
                return Ok(feedbacks);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // CREATE Feedback
        [HttpPost("feedback/CreateFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackDto feedbackDto)
        {
            try
            {
                var feedback = new Feedback
                {
                    Email = feedbackDto.Email,
                    Text = feedbackDto.Text,
                    CreatedAt = DateTime.Now
                };

                await _repository.CreateAsync(feedback);
                _redisclient.Publish("customer_feedback", $"{feedback.Email}");

                return CreatedAtAction(nameof(GetByFeedbackId), new { id = feedback.Id }, feedback);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Status = 404,
                    Detail = e.Message,
                    Message = "There is an error in creating feedback"
                });
            }
        }

        // GET FEEDBACK BY ID
        [HttpGet("feedback/GetByFeedbackId/{id}")]
        public async Task<IActionResult> GetByFeedbackId(string id)
        {
            try
            {
                var feedback = await _repository.GetByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Feedback not found"
                    });
                }

                return Ok(new
                {
                    Status = 200,
                    Data = feedback
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 404,
                    Detail = ex.Message,
                    Message = "There is an error in getting feedback"
                });
            }
        }

        // GET FEEDBACK BY EMAIL CUSTOMER
        [HttpGet("feedback/customer/{email}")]
        public async Task<IActionResult> GetByCustomerEmail(string email)
        {
            try
            {
                var feedback = await _repository.GetByCustomerEmailAsync(email);
                if (feedback == null)
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Feedback not found"
                    });
                }
                return Ok(new
                {
                    Status = 200,
                    Data = feedback
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 404,
                    Detail = ex.Message,
                    Message = "There is an error in getting feedback"
                });
            }
        }

        // DELETE Feedback
        [HttpDelete("feedback/DeleteFeedback/{id}")]
        public async Task<IActionResult> DeleteFeedback(string id)
        {
            try
            {
                var feedback = await _repository.GetByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound(new
                    {
                        Status = 404,
                        Message = "Feedback not found"
                    });
                }
                await _repository.DeleteAsync(id);
                return Ok(new
                {
                    Status = 200,
                    Message = "Feedback deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 404,
                    Detail = ex.Message,
                    Message = "There is an error in deleting feedback"
                });
            }
        }
    }
}

