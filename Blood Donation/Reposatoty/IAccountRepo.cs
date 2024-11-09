using Blood_Donation.Data;
using Blood_Donation.Models;
using Blood_Donation.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation.Repository
{
    public interface IAccountRepo
    {
        Task RegisterAsync(Account acc);
        Task<Account> GetUserByUserNameAsync(LoginViewModel model);
        Task<Account> GetAccountByIdAsync(int id); // Add this method

    }


    public class AccountRepo : IAccountRepo
    {
        private readonly BloodContext _db;

        public AccountRepo(BloodContext db)
        {
            _db = db;
        }

        public async Task RegisterAsync(Account acc)
        {
            await _db.Accounts.AddAsync(acc);
            await _db.SaveChangesAsync();
        }

        public async Task<Account> GetUserByUserNameAsync(LoginViewModel model)
        {
            return await _db.Accounts
                .FirstOrDefaultAsync(a => a.UserName == model.UserName);
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _db.Accounts
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
