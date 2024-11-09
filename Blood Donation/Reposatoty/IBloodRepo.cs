using Blood_Donation.Data;
using Blood_Donation.Models;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation.Repository
{
    public interface IBloodRepo
    {
        Task<List<Blood>> GetAllBloodsAsync();
        Task<Blood> GetBloodByIdAsync(int id);
        Task AddBloodAsync(Blood blood);
        Task DeleteBloodAsync(int id);
        Task UpdateBloodAsync(Blood blood);
        Task<IEnumerable<Blood>> GetAllBloodsByAccountIdAsync(int accountId);
    }

    public class BloodRepo : IBloodRepo
    {
        private readonly BloodContext _context;

        public BloodRepo(BloodContext context)
        {
            _context = context;
        }

        public async Task AddBloodAsync(Blood blood)
        {
            _context.Bloods.Add(blood);  // Adds the blood entity to the context
            await _context.SaveChangesAsync();  // Ensures changes are saved in the database
        }

        public async Task DeleteBloodAsync(int id)
        {
            var blood = await _context.Bloods.FindAsync(id);
            if (blood == null)
                throw new KeyNotFoundException("Blood record not found");

            // Soft delete: set Status to false
            blood.Status = false;
            _context.Bloods.Update(blood); // Update the entity
            await _context.SaveChangesAsync(); // Save changes to the database
        }


        public async Task<List<Blood>> GetAllBloodsAsync()
        {
            return await _context.Bloods
                .Where(b => b.Status)
                .ToListAsync();
        }

        public async Task<Blood> GetBloodByIdAsync(int id)
        {
            return await _context.Bloods
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateBloodAsync(Blood blood)
        {
            var existingBlood = await _context.Bloods.FindAsync(blood.Id);
            if (existingBlood == null)
            {
                throw new KeyNotFoundException("Blood record not found");
            }

            // Update the entity
            existingBlood.DonationDate = blood.DonationDate;
            existingBlood.HealthStatus = blood.HealthStatus;
            existingBlood.BloodReceiver = blood.BloodReceiver;
            existingBlood.AccountId = blood.AccountId;
            existingBlood.Status = blood.Status;

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Blood>> GetAllBloodsByAccountIdAsync(int accountId)
{
    return await _context.Bloods
        .Where(b => b.AccountId == accountId) // Filter by AccountId
        .ToListAsync();
}

    }
}
