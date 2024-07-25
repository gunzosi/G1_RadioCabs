using FeedbackServices.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackServices.Services
{
    public class FeedbackRepository
    {
        private readonly IMongoCollection<Feedback> _feedbacks;

        public FeedbackRepository(DatabaseContext context)
        {
            _feedbacks = context.Feedbacks;
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _feedbacks.Find(f => true).ToListAsync();
        }

        public async Task<Feedback> GetByIdAsync(string id)
        {
            return await _feedbacks.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Feedback> GetByCustomerEmailAsync(string email)
        {
            return await _feedbacks.Find(f => f.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Feedback feedback)
        {
            await _feedbacks.InsertOneAsync(feedback);
        }

        public async Task UpdateAsync(string id, Feedback feedback)
        {
            await _feedbacks.ReplaceOneAsync(f => f.Id == id, feedback);
        }

        public async Task DeleteAsync(string id)
        {
            await _feedbacks.DeleteOneAsync(f => f.Id == id);
        }
    }
}