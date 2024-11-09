using Blood_Donation.Models;
using Blood_Donation.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blood_Donation.Controllers
{
    public class BloodController : Controller
    {
        private readonly IBloodRepo _bloodRepo;
        private readonly IAccountRepo _accountRepo; // Ensure you have an AccountRepo for account-related operations

        public BloodController(IBloodRepo bloodRepo, IAccountRepo accountRepo)
        {
            _bloodRepo = bloodRepo;
            _accountRepo = accountRepo;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blood blood)
        {
            // Retrieve the logged-in user's Account ID from User claims
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int parsedAccountId))
            {
                blood.AccountId = parsedAccountId;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to retrieve account information.");
                return View(blood);
            }

            await _bloodRepo.AddBloodAsync(blood);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blood = await _bloodRepo.GetBloodByIdAsync(id);
            if (blood == null)
            {
                return NotFound();
            }

            return View(blood);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blood = await _bloodRepo.GetBloodByIdAsync(id);
            if (blood == null)
            {
                return NotFound();
            }

            try
            {
                await _bloodRepo.DeleteBloodAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception and/or add a model error
                ModelState.AddModelError(string.Empty, "Unable to delete the record. " + ex.Message);
                return View(blood);
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(int id)
        {
            var blood = await _bloodRepo.GetBloodByIdAsync(id);
            if (blood == null)
                return NotFound();
            return View(blood);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blood = await _bloodRepo.GetBloodByIdAsync(id);
            if (blood == null)
                return NotFound();

            // Ensure that the AccountId is set correctly
            if (blood.AccountId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid account information.");
                return View(blood);
            }

            return View(blood);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Blood blood)
        {
            // Ensure AccountId is set correctly
            if (blood.AccountId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid account information.");
                return View(blood);
            }

            // Check if the AccountId exists in the Accounts table
            var accountExists = await _accountRepo.GetAccountByIdAsync(blood.AccountId) != null;
            if (!accountExists)
            {
                ModelState.AddModelError(string.Empty, "Invalid account information.");
                return View(blood);
            }

            await _bloodRepo.UpdateBloodAsync(blood);
            return RedirectToAction("Index");
        }

        private async Task<bool> BloodExists(int id)
        {
            return await _bloodRepo.GetBloodByIdAsync(id) != null;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve the logged-in user's Account ID from User claims
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int parsedAccountId))
            {
                // Get only the blood records for the logged-in user
                var bloods = await _bloodRepo.GetAllBloodsByAccountIdAsync(parsedAccountId);

                if (bloods.Any())
                {
                    // Assuming RecentDonationDate is a static property in Blood class, set it here
                    Blood.RecentDonationDate = bloods.Max(b => b.DonationDate);
                }

                // Calculate if the user is eligible to donate based on recent donation date
                ViewBag.IsEligible = Blood.RecentDonationDate.AddDays(100) <= DateTime.Now;
                ViewBag.RecentDonationDate = Blood.RecentDonationDate;

                // Pass the filtered blood records to the view
                return View(bloods);
            }
            else
            {
                // Handle the case where the account ID is not found
                return Unauthorized();
            }
        }
    }
}
